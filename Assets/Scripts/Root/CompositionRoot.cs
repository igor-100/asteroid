using Configurations;
using Core.Camera;
using Core.ResourceEnums;
using Gameplay;
using Gameplay.Player;
using UI;
using UnityEngine;
using Utils;
namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        private static IUIRoot uiRoot;
        private static IViewFactory viewFactory;
        private static ISceneLoader sceneLoader;
        private static IResourceManager resourceManager;
        private static IConfiguration configuration;
        private static IPlayerController playerController;
        private static IPlayer ship;
        private static IOrthoCamera gameCamera;
        private static PlayerMono shipMono;

        private void OnDestroy()
        {
            configuration = null;
            uiRoot = null;
            viewFactory = null;
            playerController = null;
            ship = null;

            var resManager = GetResourceManager();
            resManager.ResetPools();
        }

        public static IResourceManager GetResourceManager()
        {
            return resourceManager ??= new ResourceManager();
        }

        public static ISceneLoader GetSceneLoader()
        {
            if (sceneLoader == null)
            {
                var resManager = GetResourceManager();
                sceneLoader = resManager.CreatePrefabInstance<ISceneLoader, EComponents>(EComponents.SceneLoader);
            }

            return sceneLoader;
        }

        public static IOrthoCamera GetGameCamera()
        {
            if (gameCamera == null)
            {
                var resManager = GetResourceManager();
                gameCamera = resManager.CreatePrefabInstance<IOrthoCamera, EComponents>(EComponents.Camera);
            }

            return gameCamera;
        }

        public static IConfiguration GetConfiguration()
        {
            return configuration ??= new Configuration();
        }

        public static IUIRoot GetUIRoot()
        {
            if (uiRoot == null)
            {
                var resManager = GetResourceManager();
                uiRoot = resManager.CreatePrefabInstance<IUIRoot, EComponents>(EComponents.UIRoot);
            }

            return uiRoot;
        }

        public static IViewFactory GetViewFactory()
        {
            if (viewFactory == null)
            {
                var root = GetUIRoot();
                var resManager = GetResourceManager();

                viewFactory = new ViewFactory(root, resManager);
            }

            return viewFactory;
        }

        public static IPlayerController GetPlayerController()
        {
            if (playerController == null)
            {
                var resManager = GetResourceManager();
                playerController =
                    resManager.CreatePrefabInstance<PlayerController, EComponents>(EComponents.PlayerController);
            }
            
            return playerController;
        }
        public static IPlayer GetShip() => ship ??= new Ship();
        public static PlayerMono GetShipMono()
        {
            if (shipMono == null)
            {
                var resManager = GetResourceManager();
                shipMono = resManager.CreatePrefabInstance<PlayerMono, EPlayers>(EPlayers.Ship);
            }
            
            return shipMono;
        }
    }
}
