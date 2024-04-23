using Configurations;
using Core.ResourceEnums;
using UI;
using UnityEngine;
namespace Core
{
    public class CompositionRoot : MonoBehaviour
    {
        private static IUIRoot UIRoot;
        private static IViewFactory ViewFactory;
        private static ISceneLoader SceneLoader;
        private static IResourceManager ResourceManager;
        private static IConfiguration Configuration;

        private void OnDestroy()
        {
            Configuration = null;
            UIRoot = null;
            ViewFactory = null;

            var resourceManager = GetResourceManager();
            resourceManager.ResetPools();
        }

        public static IResourceManager GetResourceManager()
        {
            return ResourceManager ??= new ResourceManager();

        }

        public static ISceneLoader GetSceneLoader()
        {
            if (SceneLoader == null)
            {
                var resourceManager = GetResourceManager();
                SceneLoader = resourceManager.CreatePrefabInstance<ISceneLoader, EComponents>(EComponents.SceneLoader);
            }

            return SceneLoader;
        }

        public static IConfiguration GetConfiguration()
        {
            return Configuration ??= new Configuration();
        }

        public static IUIRoot GetUIRoot()
        {
            if (UIRoot == null)
            {
                var resourceManager = GetResourceManager();
                UIRoot = resourceManager.CreatePrefabInstance<IUIRoot, EComponents>(EComponents.UIRoot);
            }

            return UIRoot;
        }

        public static IViewFactory GetViewFactory()
        {
            if (ViewFactory == null)
            {
                var uiRoot = GetUIRoot();
                var resourceManager = GetResourceManager();

                ViewFactory = new ViewFactory(uiRoot, resourceManager);
            }

            return ViewFactory;
        }
    }
}
