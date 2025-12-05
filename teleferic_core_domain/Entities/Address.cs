namespace teleferic_core_domain.Entities
{


    public class Address : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty; 
        public string? ZipCode { get; set; }    
        public bool IsDefault { get; set; }

    }
}
