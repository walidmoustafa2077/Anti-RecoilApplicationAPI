using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StripeController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private IUserService _userService;

        public StripeController(PaymentService paymentService, IUserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        [HttpPost("create-checkout-session")]
        public IActionResult CreateCheckoutSession([FromBody] PaymentRequestDTO request)
        {
            var user = HttpContext.User; // Get the authenticated user's claims

            // Extract username from the JWT token
            var username = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid token or username not found.");
            }

            // Fetch user details from the database using username
            var currentUser = _userService.GetUserDTOByUsernameOrEmailAsync(username).Result;

            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            // Prepare Stripe session details
            var customerEmail = currentUser.Email;
            var customerName = $"{currentUser.FirstName} {currentUser.LastName}";
            var invoiceId = Guid.NewGuid().ToString(); // Generate a unique invoice ID

            var session = _paymentService.CreateCheckoutSession(
                request.Price,
                request.SuccessUrl,
                request.CancelUrl,
                customerEmail,
                customerName,
                invoiceId
            );

            return Ok(new { session.Url });
        }

    }
}
