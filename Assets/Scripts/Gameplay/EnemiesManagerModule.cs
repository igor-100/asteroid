using System;
using System.Collections.Generic;
using System.Threading;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Gameplay.Player;
using Configurations.Properties;
using Cysharp.Threading.Tasks;
using Gameplay.Level.Enemies;
using Gameplay.Pool;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Gameplay.Level
{
    //TODO: Potentially could be split onto spawner, repository and manager
    public class EnemiesManagerModule
    {
        private bool isEnabled;

        private List<IEnemy> aliveEnemies;
        
        private EdgeCollider2D spawnEdges;
        private Bounds gameBounds;
        private int spawnedEnemiesCounter;
        private LevelProperties levelProperties;
        private float minSpawnDelay;
        private float maxSpawnDelay;
        private float minDelayDecrease;
        private (float min, float max) initialSpawnDelay;
        private (float min, float max) finalSpawnDelay;
        private float maxDelayDecrease;
        
        private Pool<IEnemy> enemiesPool;
        private List<EEnemies> enemiesPossibilityList;
        private IPlayer player;
        private IReadOnlyDictionary<EEnemies,EnemyProperties> enemiesProperites;
        private CancellationTokenSource cancellationTokenSource;

        public event Action<IEnemy> EnemyHitByPlayer = (enemy) => { };

        public void Init(LevelProperties levelProperties, IReadOnlyDictionary<EEnemies, EnemyProperties> enemiesProperties,
            EdgeCollider2D spawnEdges, Bounds gameBounds, IPlayer player)
        {
            this.enemiesProperites = enemiesProperties;
            this.player = player;
            this.gameBounds = gameBounds;
            this.spawnEdges = spawnEdges;

            this.levelProperties = levelProperties;
            initialSpawnDelay = levelProperties.InitialSpawnDelay;
            finalSpawnDelay = levelProperties.FinalSpawnDelay;
            minDelayDecrease = (initialSpawnDelay.min - finalSpawnDelay.min) / 
                levelProperties.TotalSpawnsToGetToTheFinalLevel;
            maxDelayDecrease = (initialSpawnDelay.max - finalSpawnDelay.max) /
                levelProperties.TotalSpawnsToGetToTheFinalLevel;
            var enemiesToSpawn = levelProperties.EnemiesToSpawn;
            enemiesPossibilityList = new List<EEnemies>();
            foreach (var valueTuple in enemiesToSpawn)
            {
                for (int i = 0; i < valueTuple.spawnWeight; i++)
                {
                    enemiesPossibilityList.Add(valueTuple.enemy);
                }
            }
            
            enemiesPool = new(() => new Enemy());
        }
        
        public void StartCycle()
        {
            spawnedEnemiesCounter = 0;

            minSpawnDelay = initialSpawnDelay.min;
            maxSpawnDelay = initialSpawnDelay.max;
            aliveEnemies = new();

            isEnabled = true;
            cancellationTokenSource = new CancellationTokenSource();
            SpawnEnemies().Forget();
        }

        private async UniTaskVoid SpawnEnemies()
        {
            while (isEnabled)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                    return;
                SpawnRandomEnemy();
                spawnedEnemiesCounter++;
                if (spawnedEnemiesCounter <= levelProperties.TotalSpawnsToGetToTheFinalLevel)
                {
                    minSpawnDelay -= minDelayDecrease;
                    maxSpawnDelay -= maxDelayDecrease;   
                }
                await UniTask.Delay((int)(Random.Range(minSpawnDelay, maxSpawnDelay) * 1000),
                    cancellationToken: cancellationTokenSource.Token);
            }
        }
        
        private void SpawnRandomEnemy()
        {
            var spawnPoint = GetRandomPointOnEdges(spawnEdges.points);

            int id = Random.Range(0, enemiesPossibilityList.Count);
            var eEnemy = enemiesPossibilityList[id];

            SpawnEnemy(eEnemy, spawnPoint);
        }
        
        private void SpawnEnemy(EEnemies eEnemy, Vector2 spawnPoint, Vector2 specifiedDirection = default)
        {
            var enemyProperties = enemiesProperites[eEnemy];

            float speed = enemyProperties.Speed;

            float rotation = 0f;
            if (enemyProperties.IsRotatedRandomly)
                rotation = Random.value * 360.0f;

            IPlayer playerToFollow = null;
            Vector2 direction = default;
            if (enemyProperties.IsFollowingPlayer)
            {
                playerToFollow = player;
            }
            else
            {
                var targetPoint = GetRandomPointInBounds2D(gameBounds);
                direction = specifiedDirection != default ? specifiedDirection : (targetPoint - spawnPoint).normalized;
            }
            
            var enemy = enemiesPool.Spawn();
            enemy.Init(eEnemy, spawnPoint, speed, rotation, direction, playerToFollow);
            enemy.GotHit += EnemyOnHit;
            
            aliveEnemies.Add(enemy);
        }

        private void EnemyOnHit(IEnemy enemy, bool isByPlayer, EHitTypes hitType)
        {
            if (enemy.EType == EEnemies.BigAsteroid && hitType == EHitTypes.Hit)
            {
                var firstDir = Random.insideUnitCircle.normalized;
                SpawnEnemy(EEnemies.SmallAsteroid, enemy.Coordinates, firstDir);
                SpawnEnemy(EEnemies.SmallAsteroid, enemy.Coordinates, -firstDir);
            }
            if (isByPlayer)
            {
                EnemyHitByPlayer(enemy);
            }
            enemy.GotHit -= EnemyOnHit;
            enemiesPool.Despawn(enemy);
            aliveEnemies.Remove(enemy);
        }
        
        public void Disable()
        {
            if (!isEnabled)
                return;
            
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();   
            }
            for (var index = aliveEnemies.Count - 1; index >= 0; index--)
            {
                var enemy = aliveEnemies[index];
                if (enemy is { IsAlive: true })
                {
                    enemy.Disable();
                }
            }
            isEnabled = false;
        }

        private static Vector2 GetRandomPointInBounds2D(Bounds bounds) {
            return new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        }
        
        private Vector2 GetRandomPointOnEdges(Vector2[] points)
        {
            int fistPointId = Random.Range(0, points.Length - 1);
            var firstPoint = points[fistPointId];
            var secondPoint = points[fistPointId + 1];
            var spawnPoint = Vector2.Lerp(firstPoint, secondPoint, Random.value);
            return spawnPoint;
        }
    }
}
