using Anti_RecoilApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Anti_RecoilApplicationAPI.Data
{
    public class AntiRecoilDbContext : DbContext
    {
        #nullable disable
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        #nullable enable
        // Constructor where options are passed, but no need to initialize Users.

        public AntiRecoilDbContext(DbContextOptions<AntiRecoilDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Create unique indexes for Email and Username
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Weapon>()
               .HasIndex(u => u.WeaponName)
               .IsUnique();
        }
    }
}
