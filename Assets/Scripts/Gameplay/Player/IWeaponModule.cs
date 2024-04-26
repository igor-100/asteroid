using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public interface IWeaponModule
    {
        int CurrentAmmo { get; }
        float ReloadTime { get; }
        
        void TryFire(Vector2 coordinates, float rotationAngle, Vector2 lookDirection);
    }
}
