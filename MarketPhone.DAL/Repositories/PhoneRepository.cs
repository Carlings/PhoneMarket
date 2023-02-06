using MarketPhone.DAL.Data;
using MarketPhone.DAL.Entities;
using MarketPhone.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MarketPhone.DAL.Repositories
{
    public class PhoneRepository : IRepository<Phone>
    {
        private PhoneDBContext db;
        IDistributedCache cache;

        public PhoneRepository(PhoneDBContext db, IDistributedCache distributedCache)
        {
            cache = distributedCache;
            this.db = db;
        }

        public void Create(Phone item)
        {
            db.Phones.Add(item);
        }

        public void Delete(int id)
        {
            Phone book = db.Phones.Find(id);
            if (book != null)
                db.Phones.Remove(book);
        }

        public IEnumerable<Phone> Find(Func<Phone, bool> predicate)
        {
            return db.Phones.Where(predicate).ToList();
        }

        public Phone Get(int id)
        {
            Phone? phone = null;
            var phoneString = cache.GetString(id.ToString());

            if (phoneString != null)
                phone = JsonSerializer.Deserialize<Phone>(phoneString);
            else
            {
                phone = db.Phones.Find(id);
                if(phone != null)
                {
                    phoneString = JsonSerializer.Serialize<Phone>(phone);
                    cache.SetString(phone.Id.ToString(), phoneString, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    });
                }
            }


            return phone;

        }

        public IEnumerable<Phone> GetAll()
        {
            return db.Phones;
        }

        public void Update(Phone item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
