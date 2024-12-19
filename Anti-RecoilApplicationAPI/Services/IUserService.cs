using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using System.Threading.Tasks;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(User user);
        Task<UserDTO> UpdateUserAsync(int userId, UserDTO userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
}
