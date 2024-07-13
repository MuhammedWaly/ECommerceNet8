using ECommerceNet8.Core.DTOS.ProductDtos.Request;
using ECommerceNet8.Infrastructure.Data.ProductModels;


namespace ECommerceNet8.DTOConvertions
{
    public static class ConvertToBase
    {

        public static BaseProduct ConvertToBaseProduct
            (this RequestBaseProduct baseProduct)
        {
            decimal totalPrice;
            decimal decimalTotalPrice;

            if(baseProduct.Discount > 0)
            {
                totalPrice = baseProduct.price -
                    (baseProduct.price * baseProduct.Discount / 100);
                decimalTotalPrice = decimal.Round(totalPrice, 2);
            }
            else
            {
                totalPrice = baseProduct.price;
                decimalTotalPrice = decimal.Round(totalPrice, 2);
            }

            var baseProductReturn = new BaseProduct()
            {
                Name = baseProduct.Name,
                Description = baseProduct.Description,
                MainCategorieId = baseProduct.MainCategoreyId,
                MaterialId = baseProduct.MaterialId,
                price = baseProduct.price,
                Discount = baseProduct.Discount,
                Totalprice = decimalTotalPrice
            };

            return baseProductReturn;
        }
    }
}
