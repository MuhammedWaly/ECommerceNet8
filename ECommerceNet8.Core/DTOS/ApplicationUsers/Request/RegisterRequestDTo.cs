using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Request
{
    public class RegisterRequestDTo
    {
        [Required]
        [StringLength(70)]
        public string FirstName {  get; set; }

        [Required]
        [StringLength(70)]
        public string LastName { get; set; }

        [Required]
        [StringLength(70)]
        [EmailAddress]
        public string EmailAddress {  get; set; }

        [Required]
        [StringLength(20,MinimumLength =8,ErrorMessage ="Your Password is limted to 8 and 25")]
        public string Password {  get; set; }
    }
}
