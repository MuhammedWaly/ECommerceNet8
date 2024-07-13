using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ProductDtos.Response
{
    public class BaseProductsPagination
    {
        public List<GetBaseProductsResponse> Products { get; set; } = new List<GetBaseProductsResponse>();

        public int? PageNumber {  get; set; }

        public int TotalPages {  get; set; }

        public string Message {  get; set; }

        public bool IsSuccess {  get; set; }

      
    }
}
