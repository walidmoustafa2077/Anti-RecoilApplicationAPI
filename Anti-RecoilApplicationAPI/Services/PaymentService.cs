using Stripe;
using Stripe.Checkout;

namespace Anti_RecoilApplicationAPI.Services
{
    public class PaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public Session CreateCheckoutSession(decimal price, string successUrl, string cancelUrl, string customerEmail, string customerName, string invoiceId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmountDecimal = price * 100, // Convert to cents
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Anti-Recoil Application",
                                Description = "Pro Edition"
                            }
                        },
                        Quantity = 1
                    }
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
                CustomerEmail = customerEmail,
                Metadata = new Dictionary<string, string>
                {
                    { "customer_name", customerName },
                    { "invoice_id", invoiceId }
                }
            };

            var service = new SessionService();
            return service.Create(options);
        }
    }
}
