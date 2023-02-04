using MarketPhone.WEB.Data;
using MarketPhone.WEB.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MarketPhone.WEB.Services
{
    public class PhoneService
    {
        PhoneDBContext DBContext;
        IDistributedCache cache;

        public PhoneService(PhoneDBContext phoneDBContext, IDistributedCache distributedCache)
        {
            DBContext = phoneDBContext;
            cache = distributedCache;
        }

        public async Task<Phone?> GetPhone(int id)
        {
            Phone? phone = null;
            var userString = await cache.GetStringAsync(id.ToString());
            if (userString != null) 
                phone = JsonSerializer.Deserialize<Phone>(userString);

            if (phone == null)
            {
                phone = await DBContext.Phones.FindAsync(id);
                if (phone != null)
                {
                    userString = JsonSerializer.Serialize(phone);

                    await cache.SetStringAsync(phone.Id.ToString(), userString, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                    });
                }
            }

            return phone;
        }
    }
}
