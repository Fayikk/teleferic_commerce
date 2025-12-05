using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace teleferic_core_domain.Entities
{
    public class ProductImage : BaseEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; } = null!;
    }
}
