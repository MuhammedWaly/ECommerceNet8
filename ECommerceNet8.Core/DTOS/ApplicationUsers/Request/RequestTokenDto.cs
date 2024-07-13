
namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Request
{
    public class RequestTokenDto
    {
        public string UserId {  get; set; }
        public string JwtToken  {get; set; }
        public string RefreshToken {  get; set; }
    }
}
