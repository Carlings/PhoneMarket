using MarketPhone.DAL.Data;
using MarketPhone.DAL.Entities;
using MarketPhone.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketPhone.DAL.Repositories
{
    public class OrderRepository : IRepository<Order>
    {
        private PhoneDBContext db;

        public OrderRepository(PhoneDBContext context)
        {
            this.db = context;
        }

        public IEnumerable<Order> GetAll()
        {
            return db.Orders.Include(o => o.Phone);
        }

        public Order Get(int id)
        {
            return db.Orders.Find(id);
        }

        public void Create(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void Update(Order order)
        {
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
        }
        public IEnumerable<Order> Find(Func<Order, Boolean> predicate)
        {
            return db.Orders.Include(o => o.Phone).Where(predicate).ToList();
        }
        public void Delete(int id)
        {
            Order order = db.Orders.Find(id);
            if (order != null)
                db.Orders.Remove(order);
            db.SaveChanges();
        }
    }
}
