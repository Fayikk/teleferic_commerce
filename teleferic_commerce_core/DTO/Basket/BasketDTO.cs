namespace teleferic_commerce_core.DTO.Basket
{
    public class BasketDTO
    {
        public string Id { get; set; } = string.Empty;
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public decimal Total => Items.Sum(item => item.Quantity * item.Price);

    }


    public class BasketItemDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class AddItemToBasketDto
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
