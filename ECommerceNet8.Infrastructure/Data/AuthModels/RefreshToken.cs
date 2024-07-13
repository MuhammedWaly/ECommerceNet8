using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.AuthModels
{
    public class RefreshToken
    {
        public int Id { get; set; } 
        public string UserId { get; set; }
        public string JwtId {  get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiresDate { get; set; } 
    }
}
