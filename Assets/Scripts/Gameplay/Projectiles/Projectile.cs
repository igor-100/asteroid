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
        private bool isToDisappear;
        private float currentDisappearTime;
        private readonly IResourceManager resourceManager;

        private bool isActive;
        private Vector2 direction;
        private float speed;
        private Vector2 coordinates;
        private EHitTypes eHitType;

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
            ProcessDisappear(obj);
        }
        
        private void ProcessMove(float deltaTime)
        {
            coordinates += speed * deltaTime * direction;
            mono.UpdateCoordinates(coordinates);
        }

        private void ProcessDisappear(float obj)
        {
            if (!isToDisappear)
                return;

            currentDisappearTime -= obj;
            if (currentDisappearTime <= 0)
            {
                Finished(this);
                currentDisappearTime = 0f;
                isToDisappear = false;
            }
        }

        public void Init(EProjectiles eProjectiles, EHitTypes hitType, Vector2 coordinates, float speed,
            float angle, Vector2 lookDirection, float disappearTime = 0f)
        {
            this.coordinates = coordinates;
            this.direction = lookDirection;
            this.speed = speed;
            this.eHitType = hitType;
            
            mono = resourceManager.GetPooledObject<ProjectileMono, EProjectiles>(eProjectiles);
            mono.Collided += MonoOnCollided;
            mono.Init(this.coordinates, angle);
            
            isToDisappear = disappearTime > 0;
            if (isToDisappear)
                this.currentDisappearTime = disappearTime;
            
            isActive = true;
        }
        
        private void MonoOnCollided(Collider2D col, IHittable hittable)
        {
            if (col.CompareTag(TagConstants.ENEMY))
            {
                hittable?.Hit(eHitType);
                if (!isToDisappear)
                    Finished(this);
            }
            if (col.CompareTag(TagConstants.DESTROY_BORDERS))
            {
                if (!isToDisappear)
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
