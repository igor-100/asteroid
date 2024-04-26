﻿using Asteroid.Core.Updater;
using Asteroid.UI.Controller;
using Configurations;
using Core.Camera;
using Core.ResourceEnums;
using UI;
using UI.Pause;
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
        private static IOrthoCamera gameCamera;
        private static IUpdater updater;

        private static IPauseScreen pauseScreen;
        private static IUiPlayerInput uiPlayerInput;

        private void OnDestroy()
        {
            configuration = null;
            uiRoot = null;
            viewFactory = null;
            updater = null;
            gameCamera = null;
            pauseScreen = null;
            uiPlayerInput = null;

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

        public static IUpdater GetUpdater()
        {
            return updater ??= MonoExtensions.CreateComponent<Updater>();
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

        public static IPauseScreen GetPauseScreen()
        {
            if (pauseScreen == null)
            {
                pauseScreen = MonoExtensions.CreateComponent<PauseScreen>();
                pauseScreen.Init(GetUiPlayerInput(), GetSceneLoader(), GetViewFactory());
            }
            
            return pauseScreen;
        }

        public static IUiPlayerInput GetUiPlayerInput()
        {
            if (uiPlayerInput == null)
            {
                var resManager = GetResourceManager();
                uiPlayerInput = resManager.CreatePrefabInstance<IUiPlayerInput, EComponents>(EComponents.UiPlayerInput);
            }
            
            return uiPlayerInput;
        }
    }
}
