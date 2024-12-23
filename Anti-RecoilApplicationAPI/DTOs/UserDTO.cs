using System.Text.Json.Serialization;

namespace Anti_RecoilApplicationAPI.DTOs
{
    public class UserDTO
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonIgnore]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } = new DateTime(2024, 1, 1);
        public string Gender { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        [JsonIgnore]
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
        public DateTime EndTrialDate { get; set; } = DateTime.Today;
    }
}

