using Configurations.Properties;
namespace Configurations
{
    public interface IConfiguration
    {
        PlayerProperties PlayerProperties { get; }
        WeaponProperties GetWeapon(EWeapon weapon);
    }
}
