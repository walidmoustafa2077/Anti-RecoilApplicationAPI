namespace Anti_RecoilApplicationAPI.DTOs
{
    public class LoginRequestDTO
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
