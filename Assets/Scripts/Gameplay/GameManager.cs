using Asteroid.Gameplay.Player;
using Configurations;
using Core.ResourceEnums;
using Cysharp.Threading.Tasks;
using Gameplay.Level.Enemies;
using UI.Pause;
namespace Gameplay.Level
{
    public class GameManager : IGameManager
    {
        private IConfiguration configuration;

        private EnemiesManagerModule enemiesManager;
        private IPlayer player;
        private PlayerMono shipMono;
        private IPauseScreen pauseScreen;
        private PlayerController playerController;
        private int currentScore;

        public void Init()
        {
            var resourceManager = GameplayRoot.ResourceManager;
            var orthoCamera = GameplayRoot.OrthoCamera;
            configuration = GameplayRoot.Configuration;
            pauseScreen = GameplayRoot.PauseScreen;
            
            player = new Ship();
            playerController =
                GameplayRoot.ResourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
            playerController.SetPlayer(player);
            playerController.SetCamera(orthoCamera.MainCamera);
            playerController.IsEnabled = true;
            
            var levelProperties = configuration.LevelProperties;
            
            var levelMono = resourceManager.CreatePrefabInstance<LevelMono, ELevels>(levelProperties.LevelPrefab);
            var spawnEdges = levelMono.SpawnEdges;
            var gameBounds = levelMono.GameBorders.bounds;
            player.SetMovementBorders(gameBounds);

            enemiesManager = new();
            enemiesManager.Init(levelProperties, configuration.Enemies, spawnEdges, gameBounds, player);
            
            RunNewCycle();
            
            enemiesManager.EnemyHit += EnemiesManagerOnEnemyHit;
            pauseScreen.Paused += PauseScreenOnPaused;
            pauseScreen.Unpaused += PauseScreenOnUnpaused;
            pauseScreen.BeforeRestartHappened += BeforeRestartHappened;
        }
        
        private void EnemiesManagerOnEnemyHit(IEnemy obj)
        {
            currentScore++;
            pauseScreen.SetScoreValue(currentScore);
        }

        private void BeforeRestartHappened()
        {
            pauseScreen.Paused -= PauseScreenOnPaused;
            pauseScreen.Unpaused -= PauseScreenOnUnpaused;
            pauseScreen.BeforeRestartHappened -= BeforeRestartHappened;
            enemiesManager.Disable();
        }

        private void PauseScreenOnUnpaused()
        {
            playerController.IsEnabled = true;
        }

        private void PauseScreenOnPaused()
        {
            playerController.IsEnabled = false;
        }

        private void PlayerOnDied()
        {
            player.Died -= PlayerOnDied;
            WaitForNewCycle().Forget();
        }
        
        private async UniTaskVoid WaitForNewCycle()
        {
            enemiesManager.Disable();
            var tsc = new UniTaskCompletionSource();
            pauseScreen.Show();
            pauseScreen.SetRestartActive(false);
            pauseScreen.WaitForResume(tsc);
            await tsc.Task;
            
            RunNewCycle();
        }
        
        private void RunNewCycle()
        {
            currentScore = 0;
            player.Init(shipMono);
            player.Died += PlayerOnDied;
            enemiesManager.StartCycle();
        }
    }
}
