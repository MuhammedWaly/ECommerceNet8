using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceNet8.Infrastructure.Data.ProductModels
{
    public class ImageBase
    {
        [Key]
        [Column(TypeName ="bigint")]
        public long Id { get; set; }

        [Column(TypeName ="varchar(max)")]
        public string ImahePath { get; set; }

        public DateTime AddedOn {  get; set; } = DateTime.UtcNow;

        public int BaseProductId {  get; set; }

        [JsonIgnore]
        public BaseProduct BaseProduct {  get; set; }

        public string StaticPath {  get; set; }
    }
}
