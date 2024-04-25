using Gameplay;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public class Ship : IPlayer
    {
        private PlayerMono mono;
        
        private float increaseSpeedAcceleration;
        private float decreaseSpeedAcceleration;
        private float rotationAcceleration;
        private float inertiaAcceleration;
        private float maximumSpeed;

        private WeaponModule weaponModule1;
        private WeaponModule weaponModule2;
        
        private float currentSpeed;
        private Vector2 coordinates;
        private Vector2 lookDirection;
        private Vector2 inertiaDirection;
        private float rotationAngle;

        public void Init(PlayerMono playerMono)
        {
            var configuration = GameplayRoot.Configuration;
            
            this.mono = playerMono;
            var playerProps = configuration.PlayerProperties;
            weaponModule1 = new(playerProps.Weapon1, configuration.GetWeapon(playerProps.Weapon1));
            weaponModule2 = new(playerProps.Weapon2, configuration.GetWeapon(playerProps.Weapon2));
            
            coordinates = Vector2.zero;
            lookDirection = Vector2.up;
            increaseSpeedAcceleration = playerProps.IncreaseSpeedAcceleration;
            maximumSpeed = playerProps.MaximumSpeed;
            decreaseSpeedAcceleration = playerProps.DecreaseSpeedAcceleration;
            rotationAcceleration = playerProps.RotationAcceleration;
            inertiaAcceleration = playerProps.InertiaAcceleration;
        }

        public void IncreaseSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maximumSpeed, increaseSpeedAcceleration);
            inertiaDirection = Vector2.Lerp(inertiaDirection, lookDirection, inertiaAcceleration);
            UpdatePosition();
        }

        public void DecreaseSpeed()
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, decreaseSpeedAcceleration);
            UpdatePosition();
        }
        
        private void UpdatePosition()
        {
            var coordinatesDelta = inertiaDirection * currentSpeed;
            coordinates += coordinatesDelta;
            mono.UpdateCoordinates(coordinates);
        }
        
        public void LookAt(Vector2 lookPos)
        {
            var targetDirection = (lookPos - coordinates).normalized;
            lookDirection = Vector2.Lerp(lookDirection, targetDirection, rotationAcceleration).normalized;
            rotationAngle = Vector2.SignedAngle(Vector2.up, lookDirection);
            mono.UpdateRotation(rotationAngle);
        }
        
        public void TryFireAttack1() => weaponModule1.TryFire(coordinates, rotationAngle);
        public void TryFireAttack2() => weaponModule2.TryFire(coordinates, rotationAngle);

        public void Hit()
        {
            mono.gameObject.SetActive(false);
        }
    }
}
