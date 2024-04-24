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
                MaximumSpeed = 0.03f,
                RotationAcceleration = 0.03f,
                InertiaAcceleration = 0.03f,
            };
        }
    }
}
