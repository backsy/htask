using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HTask.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace HTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly SectorsContext _context;
        private const string SessionNameInUse = "_NameInUse";
        private const string SessionSuccess = "_Success";

        public HomeController(SectorsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // TODO read user data back from DB, just give the two flags in session data
            var vm = new SectorViewModel();
            IQueryable<Sector> sectorQuery = from s in _context.Sector select s;
            var allSectors = new List<Sector>(await sectorQuery.Include(e => e.Children).ToListAsync());

            IQueryable<User> userQuery = from u in _context.User select u;
            var userData = await userQuery
                .Where(u => u.SessionID == HttpContext.Session.Id)
                .Include(u => u.UserSectors)
                .FirstOrDefaultAsync();
            var checkedBoxes = new List<int>();
            if(userData != null)
            { 
                foreach(var sector in userData.UserSectors)
                {
                    checkedBoxes.Add(sector.SectorId);
                }
                vm.Name = userData.UserName;
                vm.TermsAndConditions = userData.AgreeToTerms;
            }
            var checkboxListItems = RecurciveCheckboxBuilder(allSectors.Where(x => x.Parent == null), 0, checkedBoxes);
            vm.Sectors = checkboxListItems;

            if(HttpContext.Session.TryGetValue(SessionNameInUse, out var nameInUse))
                ViewBag.NameInUse = BitConverter.ToBoolean(nameInUse);
            else 
                ViewBag.NameInUse = false;
            if(HttpContext.Session.TryGetValue(SessionSuccess, out var success))
                ViewBag.Success = BitConverter.ToBoolean(success);
            else
                ViewBag.Success = false;

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
                var userById = await userQuery.Where(u => u.SessionID == id).Include(u => u.UserSectors).FirstOrDefaultAsync();
                var userByName = await userQuery.Where(u => u.UserName == vm.Name).FirstOrDefaultAsync();
                var sessionExists = userById != null;
                var nameInTable = userByName != null;
                var bothInTable = sessionExists && nameInTable;
                var noNameConflict = bothInTable ? userByName.Id == userById.Id : !nameInTable;

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
                        HttpContext.Session.Clear();
                        return new StatusCodeResult(422);
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
                        HttpContext.Session.Clear();
                        return new StatusCodeResult(422);
                    }
                }
                else
                {
                    HttpContext.Session.Set(SessionSuccess, BitConverter.GetBytes(false));
                    HttpContext.Session.Set(SessionNameInUse, BitConverter.GetBytes(true));
                    return RedirectToAction("Index"); 
                }                       
                
                HttpContext.Session.Set(SessionSuccess, BitConverter.GetBytes(true));
                HttpContext.Session.Set(SessionNameInUse, BitConverter.GetBytes(false));
                return RedirectToAction("Index"); 
            }
            HttpContext.Session.Clear();
            return new StatusCodeResult(422);
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
    }
}
