using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Forwarding;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Create a new user
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = await _userService.CreateUserAsync(userDto);
            return Ok(newUser);
        }

        [HttpPut("UpdateUser")]
        // Update an existing user
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var userContext = HttpContext.User; // Get the authenticated user's claims

                // Extract username from the JWT token
                var usernameContext = userContext?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var user = await _userService.GetUserByUsernameOrEmailAsync(usernameContext);
                // Convert the Option string to the enum value
                if (Enum.TryParse<UpdateUserOption>(request.Option, true, out var updateOption))
                {
                    // Call the update user service method with enum value
                    var userDto = await _userService.UpdateUserAsync(
                        user.UserId,
                        updateOption, // Use the enum value
                        request.NewValue
                    );

                    return Ok(userDto); // Return the updated user DTO
                }
                else
                {
                    return BadRequest(new { message = "Invalid update option." });
                }
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message if update fails
            }
        }

        [HttpDelete("DeleteUser")]
        // Delete a user
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteUser([FromBody] RemoveUserRequestDTO request)
        {
            var userContext = HttpContext.User; // Get the authenticated user's claims

            // Extract username from the JWT token
            var usernameContext = userContext?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userService.GetUserByUsernameOrEmailAsync(usernameContext);

            var result = await _userService.DeleteUserAsync(user);

            if (!result)
                return NotFound(new { message = "User not found" });

            return Ok(new { message = "User deleted successfully" });
        }

        // Get a user by ID
        [HttpGet("{GetUserById}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null || string.IsNullOrEmpty(user.Username))
                return NotFound(new { message = "User not found" });

            return Ok(user);
        }

        // Get all users (DTO format)
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserDTOs()
        {
            var users = await _userService.GetAllUserDTOsAsync();
            return Ok(users);
        }

        // Get all users (full user format)
        [HttpGet("all-full")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

    }
}
