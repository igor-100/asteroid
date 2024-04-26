using System;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Core.Updater;
using Configurations.Properties;
using Constants;
using Core;
using UnityEngine;
namespace Gameplay.Level.Enemies
{
    public class AsteroidEnemy : IAsteroidEnemy
    {
        private EnemyMono enemyMono;
        private readonly IUpdater updater;
        private readonly IResourceManager resourceManager;
        private bool isActive;
        private Vector2 coordinates;
        private float speed;
        private Vector2 movementDirection;
        private float rotationAngle;

        private EState state; 
        
        public event Action<IAsteroidEnemy, bool> Destroyed = (ast, isByPlayer) => { };

        public AsteroidEnemy()
        {
            updater = GameplayRoot.Updater;
            resourceManager = GameplayRoot.ResourceManager;
            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }

        public void Init(EEnemies eEnemies, Vector2 coordinates, float speed, float rotationAgle, Vector2 movementDirection)
        {
            this.coordinates = coordinates;
            this.speed = speed;
            this.rotationAngle = rotationAgle;
            this.movementDirection = movementDirection;
            
            enemyMono = resourceManager.GetPooledObject<EnemyMono, EEnemies>(eEnemies);
            enemyMono.Collided += MonoOnCollided;
            enemyMono.Init(this.coordinates, rotationAgle, this);
        }
        
        private void MonoOnCollided(Collider2D col)
        {
            if (col.CompareTag(TagConstants.ENEMY))
                return;
            if (!col.CompareTag(TagConstants.DESTROY_BORDERS))
                return;
            Destroyed(this, false);
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
            coordinates += speed * deltaTime * movementDirection;
            enemyMono.UpdateCoordinates(coordinates);
        }

        public void Hit(EHitTypes hitTypes)
        {
            Destroyed(this, true);
            // switch (hitTypes)
            // {
            //     case EHitTypes.Hit:
            //         break;
            //     case EHitTypes.Destroy:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException(nameof(hitTypes), hitTypes, null);
            // }
        }
        
        public void OnSpawned() => isActive = true;
        public void OnDespawned()
        {
            enemyMono.Collided -= MonoOnCollided;
            enemyMono.gameObject.SetActive(false);
            isActive = false;
        }

        enum EState
        {
            Big,
            Small,
            Inactive,
        }
    }
}
