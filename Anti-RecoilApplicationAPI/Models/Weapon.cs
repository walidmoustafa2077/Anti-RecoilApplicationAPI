using System.ComponentModel.DataAnnotations;

namespace Anti_RecoilApplicationAPI.Models
{
    public class Weapon
    {
        [Key]
        public int WeaponId { get; set; }

        [Required]
        public string WeaponName { get; set; } = string.Empty;

        [Required]
        public float FireRate { get; set; }

        [Required]
        public string Pattern { get; set; } = string.Empty; // Store pattern as a comma-separated string
    }
}
