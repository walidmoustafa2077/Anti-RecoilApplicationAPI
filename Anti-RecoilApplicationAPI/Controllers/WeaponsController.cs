using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Anti_RecoilApplicationAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponsController : ControllerBase
    {
        private readonly IWeaponService _weaponService;
        private IUserService _userService;

        public WeaponsController(IWeaponService weaponService, IUserService userService)
        {
            _weaponService = weaponService;
            _userService = userService;
        }

        // GET: api/weapons
        [HttpGet]
        public async Task<ActionResult<List<WeaponDTO>>> GetWeapons()
        {

            var user = await _userService.GetCurrentUserAsync(User); // Get current user from token
            if (user == null)
                return Unauthorized("User is not logged in.");

            // If user is not admin, check LicenseType
            if (user.Role != "Admin")
            {
                if (user.LicenseType == "Free" && user.EndTrialDate <= DateTime.UtcNow)
                {
                    return Unauthorized("Trial period expired.");
                }
            }

            // Get weapons after validation
            var weapons = await _weaponService.GetWeaponsAsync();
            if (weapons == null || !weapons.Any())
                return NotFound("No weapons found.");

            return Ok(weapons); // Return weapons to the user
        }

        // GET: api/weapons/{weaponName}
        [HttpGet("{weaponName}")]
        public async Task<ActionResult<WeaponDTO>> GetWeaponByName(string weaponName)
        {
            var weapon = await _weaponService.GetWeaponByNameAsync(weaponName);
            if (weapon == null)
                return NotFound($"Weapon with name '{weaponName}' not found.");
            return Ok(weapon);
        }

        // POST: api/weapons
        [HttpPost]
        public async Task<ActionResult<WeaponDTO>> CreateWeapon(WeaponDTO createWeaponDto)
        {

            // Check if the pattern contains newline characters and handle them.
            if (createWeaponDto.Pattern.Contains("\n") || createWeaponDto.Pattern.Contains("\r"))
            {
                // If the input contains newlines, perform the conversion
                createWeaponDto.Pattern = createWeaponDto.Pattern.Replace("\n", ",").Replace("\r", "").TrimEnd(',');
            }

            var weapon = await _weaponService.CreateWeaponAsync(createWeaponDto);

            if (weapon == null)
                return Conflict($"Weapon with name '{createWeaponDto.WeaponName}' already exists.");

            return CreatedAtAction(nameof(GetWeaponByName), new { weaponName = weapon.WeaponName }, weapon);
        }

        // PUT: api/weapons/{weaponName}
        [HttpPut("{weaponName}")]
        public async Task<ActionResult<WeaponDTO>> UpdateWeapon(string weaponName, WeaponDTO updateWeaponDto)
        {
            Console.WriteLine($"PUT Request received for: {weaponName}");

            // Check if the pattern contains newline characters and handle them.
            if (updateWeaponDto.Pattern.Contains("\n") || updateWeaponDto.Pattern.Contains("\r"))
            {
                // If the input contains newlines, perform the conversion
                updateWeaponDto.Pattern = updateWeaponDto.Pattern.Replace("\n", ",").Replace("\r", "").TrimEnd(',');
            }

            var updatedWeapon = await _weaponService.UpdateWeaponAsync(weaponName, updateWeaponDto);

            if (updatedWeapon == null)
                return NotFound($"Weapon with name '{weaponName}' not found.");

            return Ok(updatedWeapon);
        }


        // DELETE: api/weapons/{weaponName}
        [HttpDelete("{weaponName}")]
        public async Task<ActionResult> DeleteWeapon(string weaponName)
        {
            var result = await _weaponService.DeleteWeaponAsync(weaponName);

            if (!result)
                return NotFound($"Weapon with name '{weaponName}' not found.");

            return NoContent(); // Successfully deleted
        }
    }
}
