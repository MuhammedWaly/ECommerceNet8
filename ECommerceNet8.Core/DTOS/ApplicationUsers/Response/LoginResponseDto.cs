using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Response
{
    public class LoginResponseDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        public bool Result { get; set; }
    }
}
