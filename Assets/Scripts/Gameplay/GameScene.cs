using System;
using Core;
using Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public class GameScene : MonoBehaviour
    {
        private IPlayerController playerController;
        
        private void Awake()
        {
            var camera = CompositionRoot.GetGameCamera();
            var ship = CompositionRoot.GetShip();
            playerController = CompositionRoot.GetPlayerController();
            PlayerMono playerMono = CompositionRoot.GetShipMono();
            ship.Init(playerMono);
            playerController.SetPlayer(ship);
            playerController.SetCamera(camera.MainCamera);
            
            playerController.IsEnabled = true;
        }
    }
}
