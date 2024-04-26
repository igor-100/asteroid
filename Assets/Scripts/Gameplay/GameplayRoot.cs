using Asteroid.Core.Updater;
using Configurations;
using Core;
using Core.Camera;
using UI.Pause;
namespace Gameplay
{
    public class GameplayRoot
    {
        public static IUpdater Updater { get; private set; }
        public static IConfiguration Configuration { get; private set; }
        public static IResourceManager ResourceManager { get; private set; }
        public static IOrthoCamera OrthoCamera { get; private set; }
        public static IPauseScreen PauseScreen { get; private set; }
        
        public void Init(IResourceManager resourceManager, IConfiguration configuration, IOrthoCamera orthoCamera,
            IUpdater updater, IPauseScreen pauseScreen)
        {
            GameplayRoot.ResourceManager = resourceManager;
            GameplayRoot.Updater = updater;
            GameplayRoot.Configuration = configuration;
            GameplayRoot.OrthoCamera = orthoCamera;
            GameplayRoot.PauseScreen = pauseScreen;
        }
    }
}
