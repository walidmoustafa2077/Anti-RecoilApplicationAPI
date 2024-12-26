namespace Anti_RecoilApplicationAPI.DTOs
{
    public class ForgetPasswordRequestDTO
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string RetypedPassword { get; set; } = string.Empty;
    }
}
