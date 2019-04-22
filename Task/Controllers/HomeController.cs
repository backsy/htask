using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HTask.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System;
using System.Runtime.InteropServices;

namespace HTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly SectorsContext _context;
        private const string SessionUserName = "_Name";
        private const string SessionCheckedBoxes = "_CheckedBoxes";
        private const string SessionTerms = "_Terms";
        private const string SessionNameInUse = "_NameInUse";
        private const string SessionSuccess = "_Success";

        public HomeController(SectorsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new SectorViewModel();
            IQueryable<Sector> sectorQuery = from s in _context.Sector select s;
            var allSectors = new List<Sector>(await sectorQuery.Include(e => e.Children).ToListAsync());
            var checkedBoxes = new List<int>();
            if(HttpContext.Session.TryGetValue(SessionCheckedBoxes, out var checkedBoxesBytes))
                checkedBoxes = ToListOf(checkedBoxesBytes, BitConverter.ToInt32);
            if(HttpContext.Session.TryGetValue(SessionUserName, out var nameBytes))
                vm.Name = Encoding.ASCII.GetString(nameBytes);
            if(HttpContext.Session.TryGetValue(SessionTerms, out var terms))
                vm.TermsAndConditions = BitConverter.ToBoolean(terms);
            if(HttpContext.Session.TryGetValue(SessionNameInUse, out var nameInUse))
                ViewBag.NameInUse = BitConverter.ToBoolean(nameInUse);
            else 
                ViewBag.NameInUse = false;
            if(HttpContext.Session.TryGetValue(SessionSuccess, out var success))
                ViewBag.Success = BitConverter.ToBoolean(success);
            else
                ViewBag.Success = false;
            var checkboxListItems = RecurciveCheckboxBuilder(allSectors.Where(x => x.Parent == null), 0, checkedBoxes);
            vm.Sectors = checkboxListItems;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SectorViewModel vm)
        {
            var selectedSectorIDs = vm.Sectors.Where(x => x.IsChecked).Select(x => x.ID).ToList();
            if(ModelState.IsValid)
            {
                var id = HttpContext.Session.Id;
                IQueryable<Sector> sectorQuery = from s in _context.Sector select s;
                IQueryable<User> userQuery = from s in _context.User select s;
                var selectedSectors = new List<Sector>(await sectorQuery.Where(s => selectedSectorIDs.Contains(s.Id)).Include(us => us.UserSectors).ToListAsync());
                var userById = await userQuery.Where(u => u.SessionID == id).FirstOrDefaultAsync();
                var userByName = await userQuery.Where(u => u.UserName == vm.Name).FirstOrDefaultAsync();
                var sessionExists = userById != null;
                var nameInTable = userByName != null;
                var bothInTable = sessionExists && nameInTable;
                var noNameConflict = bothInTable ? userByName.Id == userById.Id : !nameInTable;
                // User exists and session is valid, update values
                if(sessionExists && noNameConflict)
                {
                    userById.AgreeToTerms = vm.TermsAndConditions;
                    userById.UserName = vm.Name;
                    if(userById.UserSectors != null)
                    {
                        userById.UserSectors.Clear();
                        _context.SaveChanges();
                    }
                    else
                        userById.UserSectors = new List<UserSector>();
                    foreach(var sector in selectedSectors)
                    {
                        userById.UserSectors.Add(new UserSector() { Sector = sector});
                    }
                    if(TryValidateModel(userById))
                    {
                        _context.Update(userById);
                        _context.SaveChanges();
                    }
                    else
                    {
                        //Crash and burn
                        HttpContext.Session.Clear();
                        return RedirectToAction("Index"); 
                    }
                }
                else if(!sessionExists && noNameConflict)
                {                    
                    var newUser = new User
                    {
                        UserName = vm.Name,
                        AgreeToTerms = vm.TermsAndConditions,
                        SessionID = id,
                        UserSectors = new List<UserSector>()
                    };
                    foreach (var sector in selectedSectors)
                    {
                        newUser.UserSectors.Add(new UserSector() { Sector = sector});
                    }
                    if(TryValidateModel(newUser))
                    {
                        _context.User.Add(newUser);
                        _context.SaveChanges();
                    }
                    else
                    {
                        //Crash and burn
                        HttpContext.Session.Clear();
                        return RedirectToAction("Index"); 
                    }
                }
                else
                {

                    HttpContext.Session.Set(SessionUserName, new byte[0]);
                    HttpContext.Session.Set(SessionCheckedBoxes, selectedSectorIDs.SelectMany(BitConverter.GetBytes).ToArray());
                    HttpContext.Session.Set(SessionTerms, BitConverter.GetBytes(vm.TermsAndConditions));
                    HttpContext.Session.Set(SessionSuccess, BitConverter.GetBytes(false));
                    HttpContext.Session.Set(SessionNameInUse, BitConverter.GetBytes(true));
                    return RedirectToAction("Index"); 
                }                       
                
                HttpContext.Session.Set(SessionUserName, Encoding.ASCII.GetBytes(vm.Name));
                HttpContext.Session.Set(SessionCheckedBoxes, selectedSectorIDs.SelectMany(BitConverter.GetBytes).ToArray());
                HttpContext.Session.Set(SessionTerms, BitConverter.GetBytes(vm.TermsAndConditions));
                HttpContext.Session.Set(SessionSuccess, BitConverter.GetBytes(true));
                HttpContext.Session.Set(SessionNameInUse, BitConverter.GetBytes(false));
                return RedirectToAction("Index"); 
            }
            //Crash and burn
            HttpContext.Session.Clear();
            return RedirectToAction("Index"); 
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<CheckBoxListItem> RecurciveCheckboxBuilder(IEnumerable<Sector> sectors, int depth, List<int> checkedSectors)
        {
            var checkBoxListItems = new List<CheckBoxListItem>();
            foreach(var sector in sectors)
            {            
                checkBoxListItems.Add(new CheckBoxListItem()
                {
                    ID = sector.Id,
                    Display = sector.Name,
                    IsChecked = checkedSectors.Contains(sector.Id),
                    Depth = depth
                });                
                var innerItems = RecurciveCheckboxBuilder(sector.Children, depth+1, checkedSectors);
                foreach(var item in innerItems)
                {
                    checkBoxListItems = checkBoxListItems.Append(item).ToList();
                }
            }
            return checkBoxListItems;
        }

        static List<T> ToListOf<T>(byte[] array, Func<byte[], int, T> bitConverter)
        {
            var size = Marshal.SizeOf(typeof(T));
            return Enumerable.Range(0, array.Length / size)
                             .Select(i => bitConverter(array, i * size))
                             .ToList();
        }
    }
}
