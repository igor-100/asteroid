using System;
using Core;
using Asteroid.Gameplay.Player;
using Gameplay.Level;
using UnityEngine;
using Utils;
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
            
            var gameplay = new GameplayRoot();
            gameplay.Init(resourceManager, configuration, gameCamera, updater);
            
            IGameManager gameManager = new GameManager();
            gameManager.Init();
        }
    }
}
