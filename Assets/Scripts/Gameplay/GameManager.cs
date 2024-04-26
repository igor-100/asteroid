using Asteroid.Gameplay.Player;
using Configurations;
using Core.ResourceEnums;
using Cysharp.Threading.Tasks;
namespace Gameplay.Level
{
    public class GameManager : IGameManager
    {
        private IConfiguration configuration;

        private EnemiesSpawnerModule enemiesSpawner;
        private IPlayer player;
        private PlayerMono shipMono;

        public void Init()
        {
            var resourceManager = GameplayRoot.ResourceManager;
            var orthoCamera = GameplayRoot.OrthoCamera;
            configuration = GameplayRoot.Configuration;
            
            player = new Ship();
            var playerController =
                GameplayRoot.ResourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
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
            
            player.Died += PlayerOnDied;
        }
        
        private void PlayerOnDied()
        {
            player.Died -= PlayerOnDied;
            NewMethod().Forget();
        }
        
        private async UniTaskVoid NewMethod()
        {
            enemiesSpawner.Disable();
            await UniTask.Delay(5000);
            player.Init(shipMono);
            player.Died += PlayerOnDied;
            enemiesSpawner.StartCycle();
        }
    }
}
