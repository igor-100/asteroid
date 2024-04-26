using Core;
using Gameplay.Level;
using UnityEngine;
namespace Gameplay
{
    public class GameScene : MonoBehaviour
    {
        private IPlayerController playerController;
        
        private void Awake()
        {
            var gameCamera = CompositionRoot.GetGameCamera();
            var configuration = CompositionRoot.GetConfiguration();
            var resourceManager = CompositionRoot.GetResourceManager();
            var updater = CompositionRoot.GetUpdater();
            var pauseScreen = CompositionRoot.GetPauseScreen();
            var gameplayHud = CompositionRoot.GetGameplayHud();
            
            var gameplay = new GameplayRoot();
            gameplay.Init(resourceManager, configuration, gameCamera, updater, pauseScreen, gameplayHud);
            
            IGameManager gameManager = new GameManager();
            gameManager.Init();
        }
    }
}
