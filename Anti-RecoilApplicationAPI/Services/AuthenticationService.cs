using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Helpers;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
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

        public async Task<UserDTO> RegisterUserAsync(
            string firstName,
            string lastName,
            string username,
            string email,
            string password,
            string retypedPassword,
            string? gender = null,
            DateTime? dateOfBirth = null,
            string? country = null,
            string? state = null,
            string? city = null)
        {
            if (password != retypedPassword)
                throw new InvalidOperationException("Passwords do not match.");

            if (await _context.Users.AnyAsync(u => u.Username == username || u.Email == email))
                throw new InvalidOperationException("Username or email is already taken.");

            var hashedPassword = PasswordHelper.HashPassword(password);

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                Email = email,
                PasswordHash = hashedPassword,
                Gender = gender ?? "",
                DateOfBirth = dateOfBirth != null ? dateOfBirth.Value.ToString("MM/dd/yyyy") : DateTime.Now.ToString("MM/dd/yyyy"),
                Country = country ?? "",
                State = state ?? "",
                City = city ?? "",
                LicenseType = "Free Trial"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser.Adapt<UserDTO>();
        }

        public async Task<string> LoginAsync(string usernameOrEmail, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid username or password.");

            // You can generate a JWT token here if needed (or any other token logic).
            return "Login successful"; // Replace with token if implemented.
        }

        public async Task<bool> ForgetPasswordAsync(string usernameOrEmail, string newPassword, string retypedPassword)
        {
            if (newPassword != retypedPassword)
                throw new InvalidOperationException("Passwords do not match.");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
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
                    user.DateOfBirth = DateTime.Parse(newValue).ToString("MM/dd/yyyy");
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

        public async Task<bool> RemoveUserAsync(string usernameOrEmail, string password)
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

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
