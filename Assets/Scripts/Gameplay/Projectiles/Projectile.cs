using System;
using Asteroid.Core.Updater;
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

        public void Init(EProjectiles eProjectiles, Vector2 coordinates, float speed, float angle, float disappearTime = 0f)
        {
            mono = resourceManager.GetPooledObject<ProjectileMono, EProjectiles>(eProjectiles);
            mono.Collided += MonoOnCollided;
            mono.Launch(coordinates, speed, angle);
            isToDisappear = disappearTime > 0;
            if (isToDisappear)
            {
                this.currentDisappearTime = disappearTime;
            }
        }
        
        private void MonoOnCollided(Collider2D col)
        {
            if (isToDisappear)
                return;
            if (col.CompareTag("Player") || col.CompareTag("Projectile"))
                return;
            Finished(this);
        }

        public void OnSpawned() { }
        
        public void OnDespawned()
        {
            mono.Collided -= MonoOnCollided;
            mono.gameObject.SetActive(false);
        }
    }
}
