using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;
using System.Security.Claims;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IAuthentication
    {
        Task<UserDTO> RegisterUserAsync(RegisterUserRequestDTO registerUserRequest);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest);
        Task<bool> ForgetPasswordAsync(ForgetPasswordRequestDTO forgetPasswordRequest);
        void InvalidateToken(ClaimsPrincipal user);
        string GenerateToken(string username, string role);
        void RemoveToken(string userId);
        bool IsTokenValid(string userId, string token);
    }
}
