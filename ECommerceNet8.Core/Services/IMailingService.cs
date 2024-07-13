using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.Services
{
    public interface IMailingService
    {


        Task SendEmailAsync(string mailTo, string Subject, string Body, IList<IFormFile> attachments = null);

    }
}
