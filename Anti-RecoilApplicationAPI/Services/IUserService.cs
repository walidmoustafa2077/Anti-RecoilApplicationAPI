using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using System.Threading.Tasks;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserDTO user);
        Task<UserDTO> UpdateUserAsync(int userId, UserDTO userDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUserDTOsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
