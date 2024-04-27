using System;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Core.Updater;
using Asteroid.Gameplay.Player;
using Configurations.Properties;
using Constants;
using Core;
using Unity.VisualScripting;
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
        public bool IsAlive { get; private set; }
        
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

        private void MonoOnCollided(Collider2D col, IHittable hittable)
        {
            if (col.CompareTag(TagConstants.DESTROY_BORDERS))
            {
                GotHit(this, false, EHitTypes.Destroy);
            }
            else if (col.CompareTag(TagConstants.PLAYER))
            {
                hittable?.Hit(EHitTypes.Destroy);
            }
        }

        private void UpdaterOnDestroyed()
        {
            updater.Updated -= UpdaterOnUpdated;
            updater.Destroyed -= UpdaterOnDestroyed;
        }

        private void UpdaterOnUpdated(float obj)
        {
            if (!IsAlive)
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
        
        public void Disable()
        {
            GotHit(this, false, EHitTypes.Destroy);
        }
        
        public void OnSpawned() => IsAlive = true;
        public void OnDespawned()
        {
            if (!enemyMono.IsUnityNull())
            {
                enemyMono.Collided -= MonoOnCollided;
                enemyMono.gameObject.SetActive(false);   
            }
            IsAlive = false;
        }
    }
}
