using System.ComponentModel.DataAnnotations;

namespace Anti_RecoilApplicationAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; } = new DateTime(2024, 1, 1);
        public string Gender { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        [Required]
        public string LicenseType { get; set; } = string.Empty;
        public DateTime EndTrialDate { get; set; }


        public string AccountStatus { get; set; } = string.Empty;

        public string LastUsedIPAddress { get; set; } = string.Empty;

        public string ResetToken { get; set; } = string.Empty;
        public DateTime? ResetTokenExpiry { get; set; }
    }


}
