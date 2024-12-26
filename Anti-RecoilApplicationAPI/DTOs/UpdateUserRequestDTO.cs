using Anti_RecoilApplicationAPI.Enums;

namespace Anti_RecoilApplicationAPI.DTOs
{
    public class UpdateUserRequestDTO
    {
        public string Option { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
