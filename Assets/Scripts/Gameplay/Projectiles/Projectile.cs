using System;
using Core;
using Core.ResourceEnums;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public class Projectile : IProjectile
    {
        private ProjectileMono mono;
        public event Action<IProjectile> Collided = (proj) => { };
        
        public void Init(Vector2 coordinates, float speed, float angle, IResourceManager resourceManager)
        {
            this.mono = resourceManager.GetPooledObject<ProjectileMono, EProjectiles>(EProjectiles.Projectile);
            mono.Collided += MonoOnCollided;
            mono.Launch(coordinates, speed, angle);
        }
        
        private void MonoOnCollided() => Collided(this);

        public void OnSpawned() { }
        
        public void OnDespawned()
        {
            mono.Collided -= MonoOnCollided;
            mono.gameObject.SetActive(false);
        }
    }
}
