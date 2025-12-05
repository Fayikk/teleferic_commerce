namespace teleferic_core_domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
        
    }
}
