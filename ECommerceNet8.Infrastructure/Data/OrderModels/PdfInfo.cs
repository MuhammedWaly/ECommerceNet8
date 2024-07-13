using ECommerceNet8.Infrastructure.Data.OrderModels;
using System.Text.Json.Serialization;

namespace ECommerceNet8.Models.OrderModels
{
    public class PdfInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public string Path { get; set; }
    }
}
