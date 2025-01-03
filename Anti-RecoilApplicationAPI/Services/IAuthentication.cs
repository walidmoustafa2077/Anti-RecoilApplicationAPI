﻿using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Enums;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IAuthentication
    {
        Task<UserDTO> RegisterUserAsync(RegisterUserRequest registerUserRequest);
        Task<LoginResponseDTO> LoginAsync(LoginRequest loginRequest);
        Task<bool> ForgetPasswordAsync(ForgetPasswordRequest forgetPasswordRequest);
        Task<UserDTO> UpdateUserAsync(string usernameOrEmail, UpdateUserOption option, string newValue, string password);
        Task<bool> RemoveUserAsync(LoginRequest loginRequest);
    }
}
