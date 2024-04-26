using System;
using Asteroid.Core.Updater;
using Asteroid.Gameplay.Player;
using Configurations.Properties;
using Constants;
using Core;
using Core.ResourceEnums;
using Gameplay;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public class Projectile : IProjectile
    {
        private ProjectileMono mono;
        private readonly IUpdater updater;
        private readonly IResourceManager resourceManager;

        private bool isActive;
        private Vector2 direction;
        private float speed;
        private Vector2 coordinates;
        private EHitTypes eHitType;
        private bool cannotBeDestroyed;

        public event Action<IProjectile> Finished = (proj) => { };

        public Projectile()
        {
            updater = GameplayRoot.Updater;
            resourceManager = GameplayRoot.ResourceManager;
            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }
        
        private void UpdaterOnDestroyed()
        {
            updater.Updated -= UpdaterOnUpdated;
            updater.Destroyed -= UpdaterOnDestroyed;
        }

        private void UpdaterOnUpdated(float obj)
        {
            if (!isActive)
                return;

            ProcessMove(obj);
        }
        
        private void ProcessMove(float deltaTime)
        {
            coordinates += speed * deltaTime * direction;
            mono.UpdateCoordinates(coordinates);
        }

        public void Init(EProjectiles eProjectiles, EHitTypes hitType, Vector2 coordinates, float speed,
            float angle, Vector2 lookDirection, bool cannotBeDestroyed = false)
        {
            this.coordinates = coordinates;
            this.direction = lookDirection;
            this.speed = speed;
            this.eHitType = hitType;
            this.cannotBeDestroyed = cannotBeDestroyed;
            
            mono = resourceManager.GetPooledObject<ProjectileMono, EProjectiles>(eProjectiles);
            mono.Collided += MonoOnCollided;
            mono.Init(this.coordinates, angle);
            
            isActive = true;
        }
        
        private void MonoOnCollided(Collider2D col, IHittable hittable)
        {
            if (col.CompareTag(TagConstants.ENEMY))
            {
                hittable?.Hit(eHitType);
                if (!cannotBeDestroyed)
                    Finished(this);
            }
            if (col.CompareTag(TagConstants.DESTROY_BORDERS))
            {
                Finished(this);
            }
        }

        public void OnSpawned() { }
        
        public void OnDespawned()
        {
            mono.Collided -= MonoOnCollided;
            mono.gameObject.SetActive(false);
            isActive = false;
        }
    }
}
