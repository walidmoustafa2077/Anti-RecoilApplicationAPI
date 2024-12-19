using Anti_RecoilApplicationAPI.Enums;

namespace Anti_RecoilApplicationAPI.DTOs
{
    // Request model for registering a user
    public class RegisterUserRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RetypedPassword { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Gender { get; set; } // Optional
        public DateTime? DateOfBirth { get; set; } // Optional
    }

    // Request model for logging in
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Request model for updating a user
    public class UpdateUserRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public UpdateUserOption Option { get; set; }
        public string NewValue { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    // Request model for removing a user
    public class RemoveUserRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty;
    }

    // Request model for forget password
    public class ForgetPasswordRequest
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RetypedPassword { get; set; } = string.Empty;
    }

}
