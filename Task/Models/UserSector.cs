namespace HTask.Models
{
    public class UserSector
    {
        public int UserId { get; set; }
        public int SectorId { get; set; }
        public User User { get; set; }
        public Sector Sector { get; set; }
    }
}
