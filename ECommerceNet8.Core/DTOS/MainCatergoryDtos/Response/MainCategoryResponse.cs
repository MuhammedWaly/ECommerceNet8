using ECommerceNet8.Infrastructure.Data.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.MainCatergoryDtos.Response
{
    public class MainCategoryResponse
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; }
        public List<MainCategorie> MainCategories { get; set; } = new List<MainCategorie>();
    }
}
