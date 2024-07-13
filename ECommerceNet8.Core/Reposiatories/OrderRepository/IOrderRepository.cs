using ECommerceNet8.DTOs.OrderDtos.Request;
using ECommerceNet8.DTOs.OrderDtos.Response;
using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Models.OrderModels;

namespace ECommerceNet8.Core.Reposiatories.OrderRepository
{
    public interface IOrderRepository
    {
        Task<string> CreatePdf(Order exsistingOrder, string UserId);
        Task<Order> GetOrderForPdf(int OrderId);

    }
}
