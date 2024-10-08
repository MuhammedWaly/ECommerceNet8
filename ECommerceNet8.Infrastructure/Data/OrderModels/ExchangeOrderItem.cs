﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Infrastructure.Data.OrderModels
{
    public class ExchangeOrderItem
    {
        public int Id { get; set; }
        public int ItemExchangeRequestId { get; set; }
        [JsonIgnore]
        public ItemExchangeRequest ItemExchangeRequest { get; set; }
        public int BaseProductId { get; set; }
        public string BaseProductName { get; set; }
        [Column(TypeName ="decimal(18,2)")]
        public decimal PricePerItemPaid { get; set; }

        public int ReturnedProductVariantId { get; set; }
        public string ReturnedProductVariantColor { get; set; }
        public string ReturnedProductVariantSize { get; set; }

        public int ExchangedProductVariantId { get; set; }
        public string ExchangedProductVariantColor { get;set; }
        public string ExchangedProductVariantSize { get; set; }

        public int Quantity { get; set; }
        public string Message { get; set; }
    }
}
