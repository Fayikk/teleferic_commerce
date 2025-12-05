using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teleferic_core_domain.Entities
{

    public class Basket
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

    }

    public class BasketItem 
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }  
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;    
    }
}
