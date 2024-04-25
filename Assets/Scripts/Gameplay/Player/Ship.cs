using Asteroid.Gameplay.Projectiles;
using Configurations.Properties;
using Core;
using Gameplay.Pool;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public class Ship : IPlayer
    {
        private PlayerMono mono;
        private IResourceManager resourceManager;
        private IPool<IProjectile> projectilesPool;
        
        private float increaseSpeedAcceleration;
        private float decreaseSpeedAcceleration;
        private float rotationAcceleration;
        private float inertiaAcceleration;
        private float maximumSpeed;
        
        private int ammoSize;
        private float projectileSpeed;
        private float reloadTime;
        private float cooldownTime;
        
        private float currentSpeed;
        private Vector2 coordinates;
        private Vector2 lookDirection;
        private Vector2 inertiaDirection;
        private float rotationAngle;

        private EGunState gunState;

        public void Init(PlayerMono playerMono, PlayerProperties playerProps, IResourceManager resourceManager)
        {
            this.mono = playerMono;
            this.resourceManager = resourceManager;
            this.projectilesPool = new Pool<IProjectile>(() => new Projectile());;
            
            coordinates = Vector2.zero;
            lookDirection = Vector2.up;
            increaseSpeedAcceleration = playerProps.IncreaseSpeedAcceleration;
            maximumSpeed = playerProps.MaximumSpeed;
            decreaseSpeedAcceleration = playerProps.DecreaseSpeedAcceleration;
            rotationAcceleration = playerProps.RotationAcceleration;
            inertiaAcceleration = playerProps.InertiaAcceleration;
            ammoSize = playerProps.AmmoSize;
            projectileSpeed = playerProps.ProjectileSpeed;
            reloadTime = playerProps.ReloadTime;
            cooldownTime = playerProps.CooldownBetweenShotsTime;
            
            gunState = EGunState.ReadyToFire;
        }
        
        public void LogicUpdate(float deltaTime)
        {
            
        }

        public void IncreaseSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maximumSpeed, increaseSpeedAcceleration);
            inertiaDirection = Vector2.Lerp(inertiaDirection, lookDirection, inertiaAcceleration);
            UpdatePosition();
        }

        public void DecreaseSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, decreaseSpeedAcceleration);
            UpdatePosition();
        }
        
        private void UpdatePosition()
        {
            var coordinatesDelta = inertiaDirection * currentSpeed;
            coordinates += coordinatesDelta;
            mono.UpdateCoordinates(coordinates);
        }
        
        public void LookAt(Vector2 lookPos)
        {
            var targetDirection = (lookPos - coordinates).normalized;
            lookDirection = Vector2.Lerp(lookDirection, targetDirection, rotationAcceleration).normalized;
            rotationAngle = Vector2.SignedAngle(Vector2.up, lookDirection);
            mono.UpdateRotation(rotationAngle);
        }
        
        public void TryFire()
        {
            if (gunState == EGunState.ReadyToFire)
            {
                Fire();
            }
        }
        
        private void Fire()
        {
            var projectile = projectilesPool.Spawn();
            projectile.Init(coordinates, projectileSpeed, rotationAngle, resourceManager);
            projectile.Collided += ProjectileOnCollided;
        }
        
        private void ProjectileOnCollided(IProjectile obj)
        {
            obj.Collided -= ProjectileOnCollided;
            projectilesPool.Despawn(obj);
        }
        
        public void Hit()
        {
            mono.gameObject.SetActive(false);
        }

        enum EGunState
        {
            ReadyToFire,
            Cooldown,
            Reload,
        }
    }
}
