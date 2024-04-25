using Configurations.Properties;
namespace Configurations
{
    public class Configuration : IConfiguration
    {
        public PlayerProperties PlayerProperties { get; }

        public Configuration()
        {
            PlayerProperties = new PlayerProperties()
            {
                IncreaseSpeedAcceleration = 0.005f,
                DecreaseSpeedAcceleration = 0.01f,
                MaximumSpeed = 0.02f,
                RotationAcceleration = 0.03f,
                InertiaAcceleration = 0.03f,
                
                CooldownBetweenShotsTime = 0.2f,
                ProjectileSpeed = 5f,
                ReloadTime = 1.5f,
                AmmoSize = 20,
            };
        }
    }
}
