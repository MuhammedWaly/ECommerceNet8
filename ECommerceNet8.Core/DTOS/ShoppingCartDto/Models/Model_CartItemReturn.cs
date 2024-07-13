using ECommerceNet8.Core.DTOS.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ShoppingCartDto.Models
{
    public class Model_CartItemReturn
    {
        public int BaseProductId {  get; set; }
        public string BaseProductName {  get; set; }
        public string BaseProductDiscription {  get; set; }
        public decimal Price {  get; set; }
        public int Discount {  get; set; }
        public decimal TotalPricePerItem { get; set; }
        public int ProductVariantId { get; set; }
        public Model_ProductColorCustom ProductColor {  get; set; }
        public Model_ProductSizeCustom ProductSize {  get; set; }
        public int AvaliableQuantity {  get; set; }
        public int SelectedQuantity {  get; set; }
        public bool CanBeSold {  get; set; }
        public decimal TotalPrice {  get; set; }
        public string Message {  get; set; }
    }
}
