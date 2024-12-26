using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;


namespace Anti_RecoilApplicationAPI.Controllers
{
    [ApiController]
    [Route("Webhook")]
    public class StripeWebhookController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly IUserService _userService;

        public StripeWebhookController(PaymentService paymentService, IUserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> HandleStripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                // Handle the event
                // If on SDK version < 46, use class Events instead of EventTypes
                if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    // Then define and call a method to handle the successful payment intent.
                    // handlePaymentIntentSucceeded(paymentIntent);
                    // Check for the event type (checkout session completed)


                    // Retrieve the user using the session ID or metadata

                    var userEmail = paymentIntent.ReceiptEmail;

                    var user = await _userService.GetUserByUsernameOrEmailAsync(userEmail);
                    if (user != null)
                    {
                        // Update the user's LicenseType to 'Pro' after payment success
                        var updateUserOption = new UpdateUserOption();
                        updateUserOption = UpdateUserOption.LicenseType;
                        await _userService.UpdateUserAsync(user.UserId, updateUserOption, "Pro");
                    }

                }
                else if (stripeEvent.Type == EventTypes.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
