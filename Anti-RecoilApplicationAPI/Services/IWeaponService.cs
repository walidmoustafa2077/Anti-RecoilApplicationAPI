using Anti_RecoilApplicationAPI.DTOs;

namespace Anti_RecoilApplicationAPI.Services
{
    public interface IWeaponService
    {
        Task<List<WeaponDTO>> GetWeaponsAsync();
        Task<WeaponDTO> GetWeaponByNameAsync(string weaponName);
        Task<WeaponDTO> CreateWeaponAsync(WeaponDTO createWeaponDto);
        Task<WeaponDTO> UpdateWeaponAsync(string weaponName, WeaponDTO updateWeaponDto);

        Task<bool> DeleteWeaponAsync(string weaponName);
    }
}
