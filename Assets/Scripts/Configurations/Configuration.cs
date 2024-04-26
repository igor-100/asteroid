using System.Collections.Generic;
using Asteroid.Configurations.ResourceEnums;
using Configurations.Properties;
using Core.ResourceEnums;
namespace Configurations
{
    //TODO: turn it into json, scriptable object or whatever you need
    public class Configuration : IConfiguration
    {
        private readonly Dictionary<EWeapon, WeaponProperties> weapons;
        private readonly Dictionary<EEnemies, EnemyProperties> enemies;
        public PlayerProperties PlayerProperties { get; }
        public LevelProperties LevelProperties { get; }
        public WeaponProperties GetWeapon(EWeapon weapon) => weapons[weapon];
        public EnemyProperties GetEnemy(EEnemies eEnemies) => enemies[eEnemies];
        public IReadOnlyDictionary<EEnemies, EnemyProperties> Enemies => enemies;

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
            LevelProperties = new LevelProperties()
            {
                LevelPrefab = ELevels.Level1,
                EnemiesToSpawn = new()
                {
                    (EEnemies.Ufo, 1), (EEnemies.BigAsteroid, 5),
                },
                InitialSpawnDelay = (2f, 4f),
                TotalSpawnsToGetToTheFinalLevel = 100,
                FinalSpawnDelay = (0.5f, 1f)
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
                    }
                },
                {
                    EWeapon.Laser, new()
                    {
                        ProjectilePrefab = EProjectiles.Laser,
                        HitType = EHitTypes.Destroy,
                        
                        AmmoSize = 3,
                        CooldownBetweenShotsTime = 0.5f,
                        ReloadTime = 7f,
                        ProjectileSpeed = 10f,
                        
                        CannotBeDestroyed = true,
                    }
                },
            };
            enemies = new()
            {
                {
                    EEnemies.SmallAsteroid, new()
                    {
                        Speed = 3f,
                        IsRotatedRandomly = true,
                        IsFollowingPlayer = false,
                    }
                },
                {
                    EEnemies.BigAsteroid, new()
                    {
                        Speed = 2f,
                        IsRotatedRandomly = true,
                        IsFollowingPlayer = false,
                    }
                },
                {
                    EEnemies.Ufo, new()
                    {
                        Speed = 2f,
                        IsRotatedRandomly = false,
                        IsFollowingPlayer = true,
                    }
                },
            };
        }
    }
}
