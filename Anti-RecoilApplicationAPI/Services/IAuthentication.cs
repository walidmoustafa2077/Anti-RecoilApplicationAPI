using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using System.Threading.Tasks;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IAuthentication
    {
        Task<UserDTO> RegisterUserAsync(
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
            string? city = null);

        Task<string> LoginAsync(string usernameOrEmail, string password);
        Task<bool> ForgetPasswordAsync(string usernameOrEmail, string newPassword, string retypedPassword);
        Task<UserDTO> UpdateUserAsync(string usernameOrEmail, UpdateUserOption option, string newValue, string password);
        Task<bool> RemoveUserAsync(string usernameOrEmail, string password);
    }
}
