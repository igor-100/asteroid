using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public interface IPlayer : IHittable 
    {
        void Init(PlayerMono playerMono);

        void IncreaseSpeed();
        void DecreaseSpeed();
        void LookAt(Vector2 lookPos);
        void TryFireAttack1();
        void TryFireAttack2();
    }
}
