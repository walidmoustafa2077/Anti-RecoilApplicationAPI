using Anti_RecoilApplicationAPI.Data;
using Anti_RecoilApplicationAPI.DTOs;
using Anti_RecoilApplicationAPI.Models;
using Mapster;
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

            if (foundWeapon == null) return new WeaponDTO();

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

            if (existingWeapon) return createWeaponDto; // Return null if weapon already exists

            var weapon = createWeaponDto.Adapt<Weapon>();

            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();

            return weapon.Adapt<WeaponDTO>();
        }

        public async Task<WeaponDTO> UpdateWeaponAsync(string weaponName, WeaponDTO updateWeaponDto)
        {
            var existingWeapon = await _context.Weapons.FirstOrDefaultAsync(w => w.WeaponName == weaponName);

            if (existingWeapon == null)
                return new WeaponDTO();

            existingWeapon.Sensitivity = updateWeaponDto.Sensitivity;
            existingWeapon.Pattern = updateWeaponDto.Pattern;

            _context.Update(existingWeapon);
            await _context.SaveChangesAsync();

            return existingWeapon.Adapt<WeaponDTO>(); // Assuming you are using AutoMapper or map manually
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
