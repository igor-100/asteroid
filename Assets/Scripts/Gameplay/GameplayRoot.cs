using Asteroid.Core.Updater;
using Asteroid.Gameplay.Player;
using Configurations;
using Core;
using Core.Camera;
using Core.ResourceEnums;
using UnityEngine;
using Utils;

namespace Gameplay
{
    public class GameplayRoot
    {
        public static IUpdater Updater { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        public static IResourceManager ResourceManager { get; private set; }
        
        private IPlayerController playerController;
        private IPlayer player;
        private PlayerMono shipMono;

        public void Init(IResourceManager resourceManager, IConfiguration configuration, IOrthoCamera orthoCamera,
            IUpdater updater)
        {
            GameplayRoot.ResourceManager = resourceManager;
            GameplayRoot.Updater = updater;
            GameplayRoot.Configuration = configuration;
            
            player = new Ship();
            playerController =
                GameplayRoot.ResourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
            player.Init(shipMono);
            playerController.SetPlayer(player);
            playerController.SetCamera(orthoCamera.MainCamera);
            playerController.IsEnabled = true;
        }
    }
}
