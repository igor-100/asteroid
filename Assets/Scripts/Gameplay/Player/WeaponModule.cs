using System;
using Asteroid.Core.Updater;
using Asteroid.Gameplay.Projectiles;
using Configurations.Properties;
using Core;
using Gameplay;
using Gameplay.Pool;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public class WeaponModule
    {
        private IPool<IProjectile> projectilesPool;
        private EGunState gunState;
        private readonly IResourceManager resourceManager;
        
        private readonly WeaponProperties weaponProperties;
        private bool isEndless;
        private float currentCooldownTimer;
        private int currentAmmo;
        private float currentReloadTimer;
        private readonly IUpdater updater;
        private readonly EWeapon eWeapon;

        public WeaponModule(EWeapon eWeapon, WeaponProperties weaponProperties)
        {
            this.resourceManager = GameplayRoot.ResourceManager;
            updater = GameplayRoot.Updater;
            
            this.eWeapon = eWeapon;
            this.projectilesPool = new Pool<IProjectile>(() => new Projectile());;
            gunState = EGunState.ReadyToFire;
            this.weaponProperties = weaponProperties;
            currentAmmo = this.weaponProperties.AmmoSize;
            isEndless = this.weaponProperties.IsEndless;

            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }
        
        private void UpdaterOnDestroyed()
        {
            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }

        private void UpdaterOnUpdated(float deltaTime)
        {
            switch (gunState)
            {
                case EGunState.ReadyToFire:
                    break;
                case EGunState.Cooldown:
                    currentCooldownTimer -= deltaTime;
                    if (currentCooldownTimer <= 0)
                        gunState = EGunState.ReadyToFire;
                    break;
                case EGunState.Reload:
                    currentReloadTimer -= deltaTime;
                    if (currentReloadTimer <= 0)
                    {
                        gunState = EGunState.ReadyToFire;
                        currentAmmo = weaponProperties.AmmoSize;
                        Debug.Log($"{eWeapon} reloaded!");
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TryFire(Vector2 coordinates, float rotationAngle)
        {
            if (gunState == EGunState.ReadyToFire)
            {
                Fire(coordinates, rotationAngle);
            }
        }
        
        private void Fire(Vector2 coordinates, float rotationAngle)
        {
            var projectile = projectilesPool.Spawn();
            float disappearTime = 0f;
            if (weaponProperties.IsToDisappearAfterTime)
                disappearTime = weaponProperties.DisappearTime;
            projectile.Init(weaponProperties.ProjectileType, coordinates, weaponProperties.ProjectileSpeed,
                rotationAngle, disappearTime);
            projectile.Finished += ProjectileOnFinished;
            if (!isEndless)
            {
                currentAmmo--;
                if (currentAmmo == 0)
                {
                    Reload();
                    return;
                }
            }
            Cooldown();
        }
        
        private void Reload()
        {
            Debug.Log($"{eWeapon} reloading...");
            gunState = EGunState.Reload;
            currentReloadTimer = weaponProperties.ReloadTime;
        }

        private void Cooldown()
        {
            gunState = EGunState.Cooldown;
            currentCooldownTimer = weaponProperties.CooldownBetweenShotsTime;
        }

        private void ProjectileOnFinished(IProjectile obj)
        {
            obj.Finished -= ProjectileOnFinished;
            projectilesPool.Despawn(obj);
        }

        enum EGunState
        {
            ReadyToFire,
            Cooldown,
            Reload,
        }
    }
}
