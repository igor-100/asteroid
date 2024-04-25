using Configurations.Properties;
using Core;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public interface IPlayer : IHittable 
    {
        void Init(PlayerMono playerMono, PlayerProperties configurationPlayerProperties,
            IResourceManager resourceManager);

        void LogicUpdate(float deltaTime);
        void IncreaseSpeed();
        void DecreaseSpeed();
        void LookAt(Vector2 lookPos);
        void TryFire();
    }
}
