using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using Anti_RecoilApplicationAPI.Helpers;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AntiRecoilDbContext _context;

        public UserService(AntiRecoilDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO user)
        {

            // Map the DTO to the User model
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash); // Make sure to implement this

            // Add the user to the database
            _context.Users.Add(user.Adapt<User>());
            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, UpdateUserOption option, string newValue)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId );

            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            switch (option)
            {
                case UpdateUserOption.FirstName:
                    user.FirstName = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.LastName:
                    user.LastName = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.Password:
                    user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newValue);
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.Username:
                    user.Username = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.Email:
                    user.Email = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.Gender:
                    user.Gender = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.DateOfBirth:
                    DateTime date;
                    DateTime.TryParse(newValue, out date);
                    user.DateOfBirth = date;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.Country:
                    user.Country = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.State:
                    user.State = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.City:
                    user.City = newValue;
                    await _context.SaveChangesAsync();
                    break;
                case UpdateUserOption.LicenseType:
                    user.LicenseType = newValue;
                    await _context.SaveChangesAsync();
                    break;
            }

            return user.Adapt<UserDTO>();
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            if (user == null) return false; // Or throw exception if needed

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user == null ? new UserDTO() : user.Adapt<UserDTO>();
        }

        public async Task<IEnumerable<UserDTO>> GetAllUserDTOsAsync()
        {
            var users = await _context.Users.ToListAsync();


            return users.Adapt<IEnumerable<UserDTO>>();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();


            return users;
        }

        public async Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                return null; // If user is not authenticated, return null

            // Extract user identifier from claims, usually the username or user ID is stored in claims
            var usernameOrEmail = user.FindFirst(ClaimTypes.NameIdentifier)?.Value; // 'sub' claim

            if (string.IsNullOrEmpty(usernameOrEmail))
                return null; // If no username or email found, return null

            // Fetch user data from the repository based on the extracted username or email
            var userEntity = await GetUserByUsernameOrEmailAsync(usernameOrEmail);

            if (userEntity == null)
                return null; // Return null if the user does not exist in the database

            // Return a UserDTO object (DTO would contain user details like username, role, license type, etc.)
            return new UserDTO
            {
                Username = userEntity.Username,
                Role = userEntity.Role,
                LicenseType = userEntity.LicenseType,
                EndTrialDate = userEntity.EndTrialDate
                // Add other properties as needed
            };
        }

        public async Task<UserDTO> GetUserDTOByUsernameOrEmailAsync(string usernameOrEmail)
        {
            // Query to find the user by either username or email (case-insensitive)
            var user = await _context.Users
                .Where(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail)
                .FirstOrDefaultAsync();

            return user.Adapt<UserDTO>(); // Return the user if found, otherwise null
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            // Query to find the user by either username or email (case-insensitive)
            var user = await _context.Users
                .Where(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail)
                .FirstOrDefaultAsync();

            return user; // Return the user if found, otherwise null
        }

    }
}
