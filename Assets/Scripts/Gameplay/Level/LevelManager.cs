using Asteroid.Configurations.ResourceEnums;
using Asteroid.Gameplay.Player;
using Core.ResourceEnums;
using Cysharp.Threading.Tasks;
using Gameplay.Level.Enemies;
using Gameplay.Pool;
using UnityEngine;
namespace Gameplay.Level
{
    public class LevelManager : ILevelManager
    {
        private IPool<IUfo> ufosPool;
        private IPool<IAsteroidEnemy> asteroidsPool;
        private bool isGameOver;
        private EdgeCollider2D spawnEdges;
        private Bounds gameBounds;

        public void Init(IPlayer player)
        {
            var resourceManager = GameplayRoot.ResourceManager;
            var levelMono = resourceManager.CreatePrefabInstance<LevelMono, ELevels>(ELevels.Level1);
            spawnEdges = levelMono.SpawnEdges;
            gameBounds = levelMono.GameBorders.bounds;
            player.SetMovementBorders(gameBounds);
            
            asteroidsPool = new Pool<IAsteroidEnemy>(() => new AsteroidEnemy());

            isGameOver = false;
            SpawnAsteroids().Forget();
        }
        
        private async UniTaskVoid SpawnAsteroids()
        {
            while (!isGameOver)
            {
                await UniTask.Delay(1000);
                SpawnAsteroid();
            }
        }
        
        private void SpawnAsteroid()
        {
            var spawnPoint = GetRandomPointOnEdges(spawnEdges.points);
            var targetPoint = GetRandomPointInBounds2D(gameBounds);
            var direction = (targetPoint - spawnPoint).normalized;

            var rotation = Random.value * 360.0f;
            var asteroidEnemy = asteroidsPool.Spawn();
            asteroidEnemy.Init(EEnemies.BigAsteroid, spawnPoint, 2f, rotation, direction);
            asteroidEnemy.Destroyed += AsteroidEnemyOnDestroyed;
        }

        private void AsteroidEnemyOnDestroyed(IAsteroidEnemy arg1, bool arg2)
        {
            arg1.Destroyed -= AsteroidEnemyOnDestroyed;
            asteroidsPool.Despawn(arg1);
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
