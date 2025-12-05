using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teleferic_commerce_infrastructure.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest paymentRequest);
    }


    public class PaymentRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "TRY";
        public PaymentCardDto Card { get; set; } = null!;
        public BuyerInfo Buyer { get; set; } = null!;
        public AddressInfo ShippingAddress { get; set; } = null!;
        public List<BasketItemInfo> Items { get; set; } = new List<BasketItemInfo>();
    }

    public class BuyerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string IdentityNumber { get; set; } = "11111111111";
        public string RegistrationAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = "Turkey";
        public string Ip { get; set; } = "85.34.78.112";
    }

    public class AddressInfo
    {
        public string ContactName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = "Turkey";
        public string Address { get; set; } = string.Empty;
    }

    public class BasketItemInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = "Product";
        public decimal Price { get; set; }
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string PaymentId { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }


    public class PaymentCardDto
    {
        public string CardHolderName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
        public string ExpireMonth { get; set; } = string.Empty;
        public string ExpireYear { get; set; } = string.Empty;
        public string Cvc { get; set; } = string.Empty;
    }

}
