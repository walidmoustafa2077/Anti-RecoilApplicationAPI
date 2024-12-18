using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Anti_RecoilApplicationAPI.Services
{
    public class WeaponService : IWeaponService
    {
        private readonly AntiRecoilDbContext _context;

        public WeaponService(AntiRecoilDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeaponDTO>> GetWeaponsAsync()
        {
            return await _context.Weapons
                .Select(w => new WeaponDTO
                {
                    WeaponName = w.WeaponName,
                    Sensitivity = w.Sensitivity,
                    Pattern = w.Pattern
                }).ToListAsync();
        }

        public async Task<WeaponDTO> GetWeaponByNameAsync(string weaponName)
        {
            // Fetch all weapons and perform case-insensitive comparison in memory
            var weapon = await _context.Weapons
                .ToListAsync(); // Get all weapons first

            var foundWeapon = weapon.FirstOrDefault(w => w.WeaponName.Equals(weaponName, StringComparison.OrdinalIgnoreCase));

            if (foundWeapon == null) return null;

            return new WeaponDTO
            {
                WeaponName = foundWeapon.WeaponName,
                Sensitivity = foundWeapon.Sensitivity,
                Pattern = foundWeapon.Pattern
            };
        }

        public async Task<WeaponDTO> CreateWeaponAsync(WeaponDTO createWeaponDto)
        {
            // Fetch all weapons and check if the weapon name exists
            var weapons = await _context.Weapons
                .ToListAsync(); // Get all weapons first

            var existingWeapon = weapons
                .Any(w => w.WeaponName.Equals(createWeaponDto.WeaponName, StringComparison.OrdinalIgnoreCase));

            if (existingWeapon) return null; // Return null if weapon already exists

            var weapon = new Weapon
            {
                WeaponName = createWeaponDto.WeaponName,
                Sensitivity = createWeaponDto.Sensitivity,
                Pattern = createWeaponDto.Pattern
            };

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            return new WeaponDTO
            {
                WeaponName = weapon.WeaponName,
                Sensitivity = weapon.Sensitivity,
                Pattern = weapon.Pattern
            };
        }

        public async Task<bool> DeleteWeaponAsync(string weaponName)
        {
            var weapon = await _context.Weapons
                .Where(w => w.WeaponName.ToLower() == weaponName.ToLower())
                .FirstOrDefaultAsync();

            if (weapon == null)
                return false;

            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
