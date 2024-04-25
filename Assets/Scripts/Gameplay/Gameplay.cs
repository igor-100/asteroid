using Asteroid.Gameplay.Player;
using Configurations;
using Core;
using Core.Camera;
using Core.ResourceEnums;
using UnityEngine;
namespace Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        private IResourceManager resourceManager;

        private IPlayerController playerController;
        private IPlayer player;
        private PlayerMono shipMono;

        public void Init(IResourceManager resourceManager, IConfiguration configuration, IOrthoCamera orthoCamera)
        {
            this.resourceManager = resourceManager;
            
            player = new Ship();
            playerController =
                this.resourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
            player.Init(shipMono, configuration.PlayerProperties, resourceManager);
            playerController.SetPlayer(player);
            playerController.SetCamera(orthoCamera.MainCamera);
            playerController.IsEnabled = true;
        }

        private void Update()
        {
            player.LogicUpdate(Time.deltaTime);
        }
    }
}
