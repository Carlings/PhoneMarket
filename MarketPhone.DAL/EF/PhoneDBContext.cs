using MarketPhone.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPhone.DAL.Data
{
    public class PhoneDBContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }


        public PhoneDBContext(DbContextOptions<PhoneDBContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Phone>().HasData
                (
                new Phone { Id = 1, Name = "Nokia Lumia 630", Company = "Nokia", Price = 220.0M },
                new Phone { Id = 2, Name = "iPhone 6", Company = "Apple", Price = 320.0M },
                new Phone { Id = 3, Name = "LG G4", Company = "lG", Price = 260.0M },
                new Phone { Id = 4, Name = "Samsung Galaxy S 6", Company = "Samsung", Price = 300.0M }
                );
        }

    }


}
