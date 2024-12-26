using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserDTO user);
        Task<UserDTO> UpdateUserAsync(int userId, Enums.UpdateUserOption option, string newValue);
        Task<bool> DeleteUserAsync(User user);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUserDTOsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<UserDTO> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<UserDTO> GetUserDTOByUsernameOrEmailAsync(string usernameOrEmail);

        Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail);

    }
}
