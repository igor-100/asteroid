using Configurations.Properties;
namespace Asteroid.Gameplay.Player
{
    public interface IHittable
    {
        void Hit(EHitTypes hitTypes);
    }
}
