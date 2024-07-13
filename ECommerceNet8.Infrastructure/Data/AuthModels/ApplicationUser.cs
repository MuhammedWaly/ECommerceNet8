using Microsoft.AspNetCore.Identity;


namespace ECommerceNet8.Infrastructure.Data.AuthModels
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
