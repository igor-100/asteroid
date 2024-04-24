using System;
using Core;
using Gameplay.Player;
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
            var ship = CompositionRoot.GetShip();
            playerController = CompositionRoot.GetPlayerController();
            var playerMono = CompositionRoot.GetShipMono();
            var configuration = CompositionRoot.GetConfiguration();
            ship.Init(playerMono, configuration.PlayerProperties);
            playerController.SetPlayer(ship);
            playerController.SetCamera(camera.MainCamera);
            playerController.IsEnabled = true;
        }
    }
}
