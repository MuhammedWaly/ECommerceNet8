using ECommerceNet8.Infrastructure.Data.ShoppingCartModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.RedisCartReposaitory
{
    public interface IRedisCartService
    {
        Task<ShoppingCart> GetCartAsync(string ApplicationUserId);
         Task SaveCartAsync(ShoppingCart cart);
        Task ClearCartAsync(string ApplicationUserId);
    }
}
