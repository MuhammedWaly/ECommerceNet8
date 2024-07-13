using ECommerceNet8.Core.DTOS.ShoppingCartDto.Request;
using ECommerceNet8.Core.DTOS.ShoppingCartDto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.ShoppingcartReposaitory
{
    public interface IShoppingcartReposaitory
    {
        Task<Response_ShoppingCart> GetAllCartItems(string UserId);
        Task<Response_ShoppingCartInfo> AddItem(string UserId, Request_ShoppingCart shoppingCartItem);
        Task<Response_ShoppingCartInfo> UpdateQty(string UserId, Request_ShoppingCart shoppingCartItem);
        Task<Response_ShoppingCartInfo> RemoveItem(string UserId, Request_ShoppingCart shoppingCartItem);
        Task<Response_ShoppingCartInfo> ClearCart(string UserId);
    }
}
