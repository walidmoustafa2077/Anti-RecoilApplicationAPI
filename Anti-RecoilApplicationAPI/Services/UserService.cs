﻿using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Anti_RecoilApplicationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AntiRecoilDbContext _context;

        public UserService(AntiRecoilDbContext context)
        {
            _context = context;
        }

        public async Task<UserDTO> CreateUserAsync(User user)
        {
            // Check if the username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email))
            {
                throw new InvalidOperationException("Username or email is already taken.");
            }

            // Map the DTO to the User model
            user.PasswordHash = HashPassword(user.PasswordHash); // Make sure to implement this

            // Add the user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<UserDTO> UpdateUserAsync(int userId, UserDTO userDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User Don't Exists.");
            }

            // Update user properties
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.DateOfBirth = userDto.DateOfBirth;
            user.Gender = userDto.Gender;
            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.LicenseType = userDto.LicenseType;

            await _context.SaveChangesAsync();

            return user.Adapt<UserDTO>();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
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

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();


            return users.Adapt<IEnumerable<UserDTO>>();
        }

        private string HashPassword(string password)
        {
            // Example using BCrypt for password hashing
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


    }
}