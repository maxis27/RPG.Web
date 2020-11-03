using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RPG.Web.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Changes> Changelog { get; set; }
        public DbSet<Work> Work { get; set; }
        public DbSet<Tavern> Tavern { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemWeapon> Weapons { get; set; }
        public DbSet<ItemArmor> Armors { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}