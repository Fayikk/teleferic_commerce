namespace teleferic_commerce_core.DTO.Address
{
    public class AddressDTO
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }


    public class CreateAddressDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }

    public class UpdateAddressDTO
    {
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string AddressLine { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
