using System;
using Core;
using Asteroid.Gameplay.Player;
using UnityEngine;
using Utils;
namespace Gameplay
{
    public class GameScene : MonoBehaviour
    {
        private IPlayerController playerController;
        
        private void Awake()
        {
            var camera = CompositionRoot.GetGameCamera();
            var configuration = CompositionRoot.GetConfiguration();
            var gameplay = MonoExtensions.CreateComponent<Gameplay>();
            var resourceManager = CompositionRoot.GetResourceManager();
            gameplay.Init(resourceManager, configuration, camera);
        }
    }
}
