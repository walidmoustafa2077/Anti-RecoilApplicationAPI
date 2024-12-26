namespace Anti_RecoilApplicationAPI.DTOs
{
    public class RemoveUserRequestDTO
    {
        public string UsernameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
