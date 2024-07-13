using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Request
{
    public class LoginRequestDto
    {
        [Required,EmailAddress]
        public string EmailAddress {  get; set; }

        [Required]
        public string Password {  get; set; }
    }
}
