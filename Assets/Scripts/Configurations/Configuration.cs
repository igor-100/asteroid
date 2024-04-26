using System.Collections.Generic;
using Configurations.Properties;
using Core.ResourceEnums;
namespace Configurations
{
    public class Configuration : IConfiguration
    {
        private readonly Dictionary<EWeapon, WeaponProperties> weapons;
        public PlayerProperties PlayerProperties { get; }
        public LevelProperties LevelProperties { get; }
        public WeaponProperties GetWeapon(EWeapon weapon) => weapons[weapon];

        public Configuration()
        {
            PlayerProperties = new PlayerProperties()
            {
                IncreaseSpeedAcceleration = 0.006f,
                DecreaseSpeedAcceleration = 0.003f,
                MaximumSpeed = 0.03f,
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
                        ProjectilePrefab = EProjectiles.Bullet,
                        HitType = EHitTypes.Hit,
                        CooldownBetweenShotsTime = 0.3f,
                        ProjectileSpeed = 15f,
                        IsEndless = true,
                    }
                },
                {
                    EWeapon.Laser, new()
                    {
                        ProjectilePrefab = EProjectiles.Laser,
                        HitType = EHitTypes.Destroy,
                        
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
