using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.PaymentDTo
{
    public class CheckoutOrderResponse
    {
        public string SessionId {  get; set; }
        public string PubKey { get; set; }
    }
}
