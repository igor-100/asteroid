using Asteroid.Core.Updater;
using Asteroid.Gameplay.Player;
using Asteroid.UI.GameplayHud;
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
        private IUpdater updater;
        private IGameplayHud hud;

        public void Init()
        {
            var resourceManager = GameplayRoot.ResourceManager;
            var orthoCamera = GameplayRoot.OrthoCamera;
            configuration = GameplayRoot.Configuration;
            pauseScreen = GameplayRoot.PauseScreen;
            updater = GameplayRoot.Updater;
            hud = GameplayRoot.GameplayHud;

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
            
            enemiesManager.EnemyHitByPlayer += EnemiesManagerOnEnemyHitByPlayer;
            pauseScreen.Paused += PauseScreenOnPaused;
            pauseScreen.Unpaused += PauseScreenOnUnpaused;
            updater.Updated += UpdaterOnUpdated;
            updater.Destroyed += UpdaterOnDestroyed;
        }

        private void UpdaterOnDestroyed()
        {
            enemiesManager.Disable();
            enemiesManager.EnemyHitByPlayer -= EnemiesManagerOnEnemyHitByPlayer;
            pauseScreen.Paused -= PauseScreenOnPaused;
            pauseScreen.Unpaused -= PauseScreenOnUnpaused;
            updater.Updated -= UpdaterOnUpdated;
            updater.Destroyed -= UpdaterOnDestroyed;
            player.Died -= PlayerOnDied;
        }

        private void UpdaterOnUpdated(float obj)
        {
            hud.SetCoordinates(player.Coordinates);
            hud.SetRotation(player.Rotation);
            hud.SetSpeed(player.Speed);
            hud.SetLaserChargesCount(player.Weapon2.CurrentAmmo);
            hud.SetLaserReloadTime(player.Weapon2.ReloadTime);
        }
        
        private void EnemiesManagerOnEnemyHitByPlayer(IEnemy obj)
        {
            currentScore++;
            pauseScreen.SetScoreValue(currentScore);
        }

        private void PauseScreenOnUnpaused()
        {
            hud.Show();
            playerController.IsEnabled = true;
        }

        private void PauseScreenOnPaused()
        {
            hud.Hide();
            playerController.IsEnabled = false;
        }

        private void PlayerOnDied()
        {
            player.Died -= PlayerOnDied;
            WaitForNewCycle().Forget();
        }
        
        private async UniTaskVoid WaitForNewCycle()
        {
            hud.Hide();
            enemiesManager.Disable();
            var tsc = new UniTaskCompletionSource();
            pauseScreen.Show();
            pauseScreen.SetResumeActive(false);
            pauseScreen.WaitForRestart(tsc);
            await tsc.Task;
            
            RunNewCycle();
        }
        
        private void RunNewCycle()
        {
            hud.Show();
            currentScore = 0;
            pauseScreen.SetScoreValue(currentScore);
            player.Init(shipMono);
            player.Died += PlayerOnDied;
            enemiesManager.StartCycle();
        }
    }
}
