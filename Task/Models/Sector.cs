using System.Collections.Generic;

namespace HTask.Models
{
    public class Sector
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Sector Parent { get; set; }
        public List<Sector> Children { get; set; } = new List<Sector>();
        public List<UserSector> UserSectors { get; set;} = new List<UserSector>();
    }
}
