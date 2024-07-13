using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Request
{
    public class PasswordResetRequest
    {
        [Required]
        public string Token {  get; set; }

        [EmailAddress,Required]
        public string Email {  get; set; }

        [StringLength(50,MinimumLength = 8)]
        public string NewPassword {  get; set; }

        [Required,StringLength(50, MinimumLength = 8), Compare("NewPassword", ErrorMessage = "NewPassword and confirmNewPassword are not match")]
        public string ConfirmNewPassword {  get; set; }
    }
}
