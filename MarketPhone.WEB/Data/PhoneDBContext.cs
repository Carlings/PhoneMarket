using MarketPhone.WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPhone.WEB.Data
{
    public class PhoneDBContext : DbContext
    {
        public PhoneDBContext(DbContextOptions<PhoneDBContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
