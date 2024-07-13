using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.RedisCartReposaitory
{
    public class RedisCartService : IRedisCartService
    {
        
        private readonly IDistributedCache _cache;

        public RedisCartService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<ShoppingCart> GetCartAsync(string ApplicationUserId)
        {
            var cartJson = await _cache.GetStringAsync(ApplicationUserId);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new ShoppingCart { ApplicationUserId = ApplicationUserId, CartItems = new List<CartItem>() };
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(cartJson);
        }

        public async Task SaveCartAsync(ShoppingCart cart)
        {
            var cartJson = JsonConvert.SerializeObject(cart);
            await _cache.SetStringAsync(cart.ApplicationUserId, cartJson);
        }

        public async Task ClearCartAsync(string ApplicationUserId)
        {
            await _cache.RemoveAsync(ApplicationUserId);
        }
    }

}
