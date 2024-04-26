using System;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Core.Updater;
using Asteroid.Gameplay.Player;
using Configurations.Properties;
using Constants;
using Core;
using UnityEngine;
namespace Gameplay.Level.Enemies
{
    public class Enemy : IEnemy
    {
        public EEnemies EType { get; private set; }
        public Vector2 Coordinates { get; private set; }
        
        private EnemyMono enemyMono;
        private readonly IUpdater updater;
        private readonly IResourceManager resourceManager;
        private bool isActive;
        
        private float speed;
        private Vector2 movementDirection;
        private float rotationAngle;
        private IPlayer playerToFollow;

        public event Action<IEnemy, bool, EHitTypes> GotHit = (ast, isByPlayer, hitType) => { };

        public Enemy()
        {
            updater = GameplayRoot.Updater;
            resourceManager = GameplayRoot.ResourceManager;
            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }

        public void Init(EEnemies eEnemies, Vector2 coordinates, float speed, float rotationAngle,
            Vector2 movementDirection = default, IPlayer playerToFollow = null)
        {
            this.EType = eEnemies;
            this.Coordinates = coordinates;
            this.speed = speed;
            this.rotationAngle = rotationAngle;
            this.playerToFollow = playerToFollow;
            this.movementDirection = movementDirection;
            
            enemyMono = resourceManager.GetPooledObject<EnemyMono, EEnemies>(eEnemies);
            enemyMono.Collided += MonoOnCollided;
            enemyMono.Init(this.Coordinates, rotationAngle, this);
        }

        private void MonoOnCollided(Collider2D col)
        {
            if (col.CompareTag(TagConstants.ENEMY))
                return;
            if (!col.CompareTag(TagConstants.DESTROY_BORDERS))
                return;
            GotHit(this, false, EHitTypes.Destroy);
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
            if (playerToFollow != null)
                movementDirection = (playerToFollow.Coordinates - Coordinates).normalized;
            
            Coordinates += speed * deltaTime * movementDirection;
            enemyMono.UpdateCoordinates(Coordinates);
        }

        public void Hit(EHitTypes hitTypes)
        {
            GotHit(this, true, hitTypes);
        }
        
        public void OnSpawned() => isActive = true;
        public void OnDespawned()
        {
            enemyMono.Collided -= MonoOnCollided;
            enemyMono.gameObject.SetActive(false);
            isActive = false;
        }
    }
}
