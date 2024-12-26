using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Helpers;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Services
{
    public class AuthenticationService : IAuthentication
    {
        private readonly AntiRecoilDbContext _context;
        private readonly IUserService _userService; 
        private readonly TokenService _tokenService;

        public AuthenticationService(AntiRecoilDbContext context, TokenService tokenService, IUserService userService)
        {
            _context = context;
            _tokenService = tokenService;
            _userService = userService;
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterUserRequestDTO registerUserRequest)
        {
            if (registerUserRequest.Password != registerUserRequest.RetypedPassword)
                throw new InvalidOperationException("Passwords do not match.");

            if (await _userService.GetUserByUsernameOrEmailAsync(registerUserRequest.Username) != null || await _userService.GetUserByUsernameOrEmailAsync(registerUserRequest.Email) != null)
                throw new InvalidOperationException("Username or email is already taken.");

            var hashedPassword = PasswordHelper.HashPassword(registerUserRequest.Password);


            var newUser = registerUserRequest.Adapt<User>();

            newUser.PasswordHash = hashedPassword;

            if (newUser.Role is "Admin" or "admin")
                newUser.LicenseType = "Pro";
            else
            {
                newUser.Role = "User";
                newUser.LicenseType = "Free";
                newUser.EndTrialDate = DateTime.UtcNow.AddHours(3);
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser.Adapt<UserDTO>();
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.UsernameOrEmail || u.Email == loginRequest.UsernameOrEmail);

            if (user == null || !PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            // Generate the JWT token
            var token = _tokenService.GenerateToken(user.Username, user.Role);
            _tokenService.AddToken(user.UserId.ToString(), token);
            // Return just the token string
            return new LoginResponseDTO 
                {
                    Token = token
                };
        }


        public string GenerateToken(string username, string role)
        {
            return _tokenService.GenerateToken(username, role);
        }

        public void RemoveToken(string userId)
        {
            _tokenService.Remove(userId);
        }

        public bool IsTokenValid(string userId, string token)
        {
            return _tokenService.IsTokenValid(userId, token);
        }

        public async Task<bool> ForgetPasswordAsync(ForgetPasswordRequestDTO forgetPasswordRequest)
        {
            if (forgetPasswordRequest.NewPassword != forgetPasswordRequest.RetypedPassword)
                throw new InvalidOperationException("Passwords do not match.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == forgetPasswordRequest.UsernameOrEmail || u.Email == forgetPasswordRequest.UsernameOrEmail);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(forgetPasswordRequest.NewPassword);
            await _context.SaveChangesAsync();

            return true;
        }
        public void InvalidateToken(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("Invalid user.");
            }

            // Add logic to invalidate the token:
            // For example, remove it from a database or in-memory store
            _tokenService.Remove(userId);
        }

    }
}
