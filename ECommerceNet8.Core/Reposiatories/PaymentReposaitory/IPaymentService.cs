using ECommerceNet8.Infrastructure.Data.CheckoutModels;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Reposiatories.PaymentReposaitory
{
    public interface IPaymentService
    {
        Task<PaymentIntent> CreatePaymentIntent(decimal amount, string currency);
    }
}
