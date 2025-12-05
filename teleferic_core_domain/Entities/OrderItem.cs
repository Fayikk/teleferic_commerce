using System.ComponentModel.DataAnnotations.Schema;

namespace teleferic_core_domain.Entities
{
    public class OrderItem : BaseEntity
    {

        public string ProductName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }   
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Subtotal => Price * Quantity;



        [ForeignKey(nameof(Order))]
        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; } = null!;
        [ForeignKey(nameof(Product))]
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!; 
    }
}
