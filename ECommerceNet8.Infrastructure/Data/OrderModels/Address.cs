using ECommerceNet8.Infrastructure.Data.AuthModels;

namespace ECommerceNet8.Infrastructure.Data.OrderModels
{
    public class Address
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int HouseNumber { get; set; }
        public int? AppartmentNumber { get; set; }  
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
