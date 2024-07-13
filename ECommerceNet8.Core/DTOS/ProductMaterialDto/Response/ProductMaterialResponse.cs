using ECommerceNet8.Infrastructure.Data.ProductModels;


namespace ECommerceNet8.Core.DTOS.ProductDtos.Response
{
    public class ProductMaterialResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<Material> Materials { get; set; } = new List<Material>();
    }
}
