using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ShoppingCartDto.Response
{
    public class Response_ShoppingCartInfo
    {
        public bool IsSuccess {  get; set; }
        public string Message {  get; set; }
        public int ProductVariantId {  get; set; }
        public int RequestQty {  get; set; }
        public int StoreQty {  get; set; }
    }
}
