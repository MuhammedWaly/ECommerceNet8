﻿using ECommerceNet8.Models.OrderModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Infrastructure.Data.OrderModels
{
    public class ItemGoodForRefund
    {
        public int Id { get; set; }
        public int ItemReturnRequestId { get; set; }
        [JsonIgnore]
        public ItemReturnRequest ItemReturnRequest { get; set; }

        public int BaseProductId { get; set; }
        public string BaseProductName { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePaidPerItem { get; set; }
        public int Quantity { get; set; }
    }
}
