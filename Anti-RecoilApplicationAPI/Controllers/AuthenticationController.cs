using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthentication _authenticationService;

        private readonly IWeaponService _weaponService;

        public AuthenticationController(IAuthentication authenticationService, IWeaponService weaponService)
        {
            _authenticationService = authenticationService;
            _weaponService = weaponService;
        }

        // Register User
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterUserAsync(
            [FromBody] RegisterUserRequestDTO request)
        {
            try
            {
                // Register the user using the service
                var userDto = await _authenticationService.RegisterUserAsync(request.Adapt<RegisterUserRequestDTO>());

                return Ok(userDto); // Return user DTO after successful registration
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message in case of failure
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDTO>> LoginAsync([FromBody] LoginRequestDTO request)
        {
            try
            {
                var user = await _authenticationService.LoginAsync(request);

                if (user == null)
                {
                    return Unauthorized("Invalid credentials");
                }


                return Ok(new LoginResponseDTO { Token = user.Token });
            }
            catch (Exception ex)
            {
                // Log the error (you can use ILogger instead of Console)
                Console.WriteLine($"Login failed: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // Forget Password
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordRequestDTO request)
        {
            try
            {
                // Call the forget password service method
                var result = await _authenticationService.ForgetPasswordAsync(request.Adapt<ForgetPasswordRequestDTO>());

                return result ? Ok("Password updated successfully.") : BadRequest("Password update failed.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message in case of failure
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Invalid user ID.");
                }

                _authenticationService.RemoveToken(userId);

                return Ok("User logged out successfully.");
            }
            catch (Exception ex)
            {
                // Log the error (use ILogger for better logging)
                Console.WriteLine($"Logout failed: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
