using Microsoft.EntityFrameworkCore;

namespace HTask.Models
{
    public class SectorsContext : DbContext
    {
        public SectorsContext(DbContextOptions<SectorsContext> options)
            : base(options)
        {
        }

        public DbSet<Sector> Sector { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserSector> UserSector { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserSector>()
                .HasKey(us => new { us.UserId, us.SectorId});
            modelBuilder.Entity<UserSector>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSectors)
                .HasForeignKey(us => us.UserId);            
            modelBuilder.Entity<UserSector>()
                .HasOne(us => us.Sector)
                .WithMany(u => u.UserSectors)
                .HasForeignKey(us => us.SectorId);
        }
    }
}
