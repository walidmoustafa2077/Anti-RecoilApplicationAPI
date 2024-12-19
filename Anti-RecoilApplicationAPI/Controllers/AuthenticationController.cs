using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
            [FromBody] RegisterUserRequest request)
        {
            try
            {
                // Register the user using the service
                var userDto = await _authenticationService.RegisterUserAsync(
                    request.FirstName,
                    request.LastName,
                    request.Username,
                    request.Email,
                    request.Password,
                    request.RetypedPassword,
                    request.Gender,
                    request.DateOfBirth,
                    request.Country,
                    request.State,
                    request.City
                );

                return Ok(userDto); // Return user DTO after successful registration
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message in case of failure
            }
        }

        // Login User
        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                // Call the login service method
                var token = await _authenticationService.LoginAsync(
                    request.UsernameOrEmail,
                    request.Password
                );

                var weapons = await _weaponService.GetWeaponsAsync();

                // Return token and weapons in the response
                return Ok(new
                {
                    Token = token,
                    Weapons = weapons
                });
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message); // Return unauthorized if login fails
            }
        }

        // Forget Password
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPasswordAsync([FromBody] ForgetPasswordRequest request)
        {
            try
            {
                // Call the forget password service method
                var result = await _authenticationService.ForgetPasswordAsync(
                    request.UsernameOrEmail,
                    request.NewPassword,
                    request.RetypedPassword
                );

                return result ? Ok("Password updated successfully.") : BadRequest("Password update failed.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message in case of failure
            }
        }

        // Update User Information
        [HttpPut("update")]
        public async Task<ActionResult<UserDTO>> UpdateUserAsync(
            [FromBody] UpdateUserRequest request)
        {
            try
            {
                // Call the update user service method
                var userDto = await _authenticationService.UpdateUserAsync(
                    request.UsernameOrEmail,
                    request.Option,
                    request.NewValue,
                    request.Password
                );

                return Ok(userDto); // Return the updated user DTO
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message if update fails
            }
        }

        // Remove User
        [HttpDelete("remove")]
        public async Task<ActionResult> RemoveUserAsync([FromBody] RemoveUserRequest request)
        {
            try
            {
                // Call the remove user service method
                var result = await _authenticationService.RemoveUserAsync(
                    request.UsernameOrEmail,
                    request.Password
                );

                return result ? Ok("User removed successfully.") : BadRequest("User removal failed.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return error message if removal fails
            }
        }
    }
}
