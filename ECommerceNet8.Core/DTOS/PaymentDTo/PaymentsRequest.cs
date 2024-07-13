using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.PaymentDTo
{
    public class PaymentsRequest
    {
        public int OrderId { get; set; }
        public string Currency { get; set; }
    }
}
