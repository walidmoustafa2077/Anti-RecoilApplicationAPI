using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Helpers;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Anti_RecoilApplicationAPI.Services
{
    public class AuthenticationService : IAuthentication
    {
        private readonly AntiRecoilDbContext _context;

        public AuthenticationService(AntiRecoilDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> RegisterUserAsync(RegisterUserRequest registerUserRequest)
        {
            if (registerUserRequest.Password != registerUserRequest.RetypedPassword)
                throw new InvalidOperationException("Passwords do not match.");

            if (await _context.Users.AnyAsync(u => u.Username == registerUserRequest.Username || u.Email == registerUserRequest.Email))
                throw new InvalidOperationException("Username or email is already taken.");

            var hashedPassword = PasswordHelper.HashPassword(registerUserRequest.Password);
            
            
            var newUser = registerUserRequest.Adapt<User>();

            newUser.PasswordHash = hashedPassword;

    
            
            if (newUser.Role is "Admin" or "admin")
                newUser.LicenseType = "Pro";
            else
            {
                newUser.LicenseType = "Free";
                newUser.EndTrialDate = DateTime.UtcNow.AddHours(3);
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser.Adapt<UserDTO>();
        }

        public async Task<string> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.UsernameOrEmail || u.Email == loginRequest.UsernameOrEmail);

            if (user == null || !PasswordHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            // You can generate a JWT token here if needed (or any other token logic).
            return "Login successful"; // Replace with token if implemented.
        }

        public async Task<bool> ForgetPasswordAsync(ForgetPasswordRequest forgetPasswordRequest)
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

        public async Task<UserDTO> UpdateUserAsync(string usernameOrEmail, UpdateUserOption option, string newValue, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid password.");
            }

            switch (option)
            {
                case UpdateUserOption.FirstName:
                    user.FirstName = newValue;
                    break;
                case UpdateUserOption.LastName:
                    user.LastName = newValue;
                    break;
                case UpdateUserOption.Password:
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newValue);
                    break;
                case UpdateUserOption.Username:
                    user.Username = newValue;
                    break;
                case UpdateUserOption.Email:
                    user.Email = newValue;
                    break;
                case UpdateUserOption.Gender:
                    user.Gender = newValue;
                    break;
                case UpdateUserOption.DateOfBirth:
                    DateTime date;
                    DateTime.TryParse(newValue, out date);
                    user.DateOfBirth = date;
                    break;
                case UpdateUserOption.Country:
                    user.Country = newValue;
                    break;
                case UpdateUserOption.State:
                    user.State = newValue;
                    break;
                case UpdateUserOption.City:
                    user.City = newValue;
                    break;
                case UpdateUserOption.LicenseType:
                    user.LicenseType = newValue;
                    break;
                default:
                    throw new InvalidOperationException("Invalid update option.");
            }

            await _context.SaveChangesAsync();
            return user.Adapt<UserDTO>();
        }

        public async Task<bool> RemoveUserAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginRequest.UsernameOrEmail || u.Email == loginRequest.Password);

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                throw new InvalidOperationException("Invalid password.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
