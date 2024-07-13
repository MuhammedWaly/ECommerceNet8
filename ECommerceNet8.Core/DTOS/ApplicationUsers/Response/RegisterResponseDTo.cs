using ECommerceNet8.Infrastructure.Data.AuthModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Response
{
    public class RegisterResponseDTo
    {
        [JsonIgnore]
        public ApplicationUser ApplicationUser {  get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string EmailAddress { get; set; }

        public bool IsSuccess { get; set; }
        
        

        public List<string> Message { get; set; } = new List<string>();
    }
}
