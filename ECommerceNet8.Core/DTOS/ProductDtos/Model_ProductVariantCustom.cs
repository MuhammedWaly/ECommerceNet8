﻿namespace ECommerceNet8.Core.DTOS.ProductDtos
{
    public class Model_ProductVariantCustom
    {
        public int Id { get; set; }
        public int BaseProductId { get; set; }
        public Model_ProductColorCustom productColor { get; set; }
        public Model_ProductSizeCustom productSize { get; set; }
        public int Quantity { get; set; }
    }
}
