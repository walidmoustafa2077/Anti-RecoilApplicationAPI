namespace Anti_RecoilApplicationAPI.DTOs
{
    public class RegisterUserRequestDTO
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
}
