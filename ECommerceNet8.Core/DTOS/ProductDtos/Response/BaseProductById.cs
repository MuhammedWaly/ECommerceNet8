using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductDtos.Response
{
    public class BaseProductById
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public GetBaseProductsResponse baseProduct {  get; set; }
    }
}
