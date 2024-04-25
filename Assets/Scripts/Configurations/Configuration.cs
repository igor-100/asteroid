using System.Collections.Generic;
using Configurations.Properties;
using Core.ResourceEnums;
namespace Configurations
{
    public class Configuration : IConfiguration
    {
        private readonly Dictionary<EWeapon, WeaponProperties> weapons;
        public PlayerProperties PlayerProperties { get; }
        public WeaponProperties GetWeapon(EWeapon weapon) => weapons[weapon];

        public Configuration()
        {
            PlayerProperties = new PlayerProperties()
            {
                IncreaseSpeedAcceleration = 0.005f,
                DecreaseSpeedAcceleration = 0.01f,
                MaximumSpeed = 0.02f,
                RotationAcceleration = 0.03f,
                InertiaAcceleration = 0.03f,
                
                Weapon1 = EWeapon.Gun,
                Weapon2 = EWeapon.Laser,
            };

            weapons = new()
            {
                {
                    EWeapon.Gun, new()
                    {
                        ProjectileType = EProjectiles.Bullet,
                        CooldownBetweenShotsTime = 0.3f,
                        ProjectileSpeed = 5f,
                        IsEndless = true,
                    }
                },
                {
                    EWeapon.Laser, new()
                    {
                        ProjectileType = EProjectiles.Laser,
                        
                        AmmoSize = 1,
                        ReloadTime = 7f,
                        
                        IsToDisappearAfterTime = true,
                        DisappearTime = 1f,
                    }
                },
            };
        }
    }
}
