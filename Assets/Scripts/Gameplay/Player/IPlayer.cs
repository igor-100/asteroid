using Configurations.Properties;
using UnityEngine;
namespace Gameplay.Player
{
    public interface IPlayer
    {
        void Init(PlayerMono playerMono, PlayerProperties configurationPlayerProperties);
        
        void IncreaseSpeed();
        void DecreaseSpeed();
        void LookAt(Vector2 lookPos);
    }
}
