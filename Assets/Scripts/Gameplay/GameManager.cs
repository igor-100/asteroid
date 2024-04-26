using Asteroid.Gameplay.Player;
using Configurations;
using Core.ResourceEnums;
namespace Gameplay.Level
{
    public class GameManager : IGameManager
    {
        private IConfiguration configuration;

        private EnemiesSpawnerModule enemiesSpawner;

        public void Init()
        {
            var resourceManager = GameplayRoot.ResourceManager;
            var orthoCamera = GameplayRoot.OrthoCamera;
            configuration = GameplayRoot.Configuration;
            
            IPlayer player = new Ship();
            var playerController =
                GameplayRoot.ResourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            var shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
            player.Init(shipMono);
            playerController.SetPlayer(player);
            playerController.SetCamera(orthoCamera.MainCamera);
            playerController.IsEnabled = true;
            
            var levelProperties = configuration.LevelProperties;
            
            var levelMono = resourceManager.CreatePrefabInstance<LevelMono, ELevels>(levelProperties.LevelPrefab);
            var spawnEdges = levelMono.SpawnEdges;
            var gameBounds = levelMono.GameBorders.bounds;
            player.SetMovementBorders(gameBounds);

            enemiesSpawner = new();
            enemiesSpawner.Init(levelProperties, configuration.Enemies, spawnEdges, gameBounds, player);
            enemiesSpawner.StartCycle();
        }
    }
}
