using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.CheckoutModels
{
    public class Checkout
    {
        public string? SessionId { get; set; }
        public string? PublishableKey { get; set; }
    }
}
