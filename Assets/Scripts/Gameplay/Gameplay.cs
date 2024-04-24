using Configurations;
using Core;
using Core.Camera;
using Core.ResourceEnums;
using Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public class Gameplay : MonoBehaviour
    {
        private IResourceManager resourceManager;
        
        private IPlayerController playerController;
        private Ship ship;
        private PlayerMono shipMono;

        public void Init(IResourceManager resourceManager, IConfiguration configuration, IOrthoCamera orthoCarmera)
        {
            this.resourceManager = resourceManager;
            ship = new();
            playerController =
                this.resourceManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            shipMono = resourceManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            
            ship.Init(shipMono, configuration.PlayerProperties);
            playerController.SetPlayer(ship);
            playerController.SetCamera(orthoCarmera.MainCamera);
            playerController.IsEnabled = true;
        }
    }
}
