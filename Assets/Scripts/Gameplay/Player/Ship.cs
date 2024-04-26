using System;
using Configurations.Properties;
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
        private Bounds bounds;
        private float boundsX;
        private float boundsY;

        public event Action Died = () => { };

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

        public void SetMovementBorders(Bounds bounds)
        {
            this.bounds = bounds;
            boundsX = bounds.extents.x;
            boundsY = bounds.extents.y;
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
            var newCoordinates = coordinates + coordinatesDelta;
            if (!bounds.Contains(newCoordinates))
            {
                if (newCoordinates.x > boundsX)
                    newCoordinates.x = -boundsX;
                else if (newCoordinates.y > boundsY)
                    newCoordinates.y = -boundsY;
                else if (newCoordinates.x < -boundsX)
                    newCoordinates.x = boundsX;
                else if (newCoordinates.y < -boundsY)
                    newCoordinates.y = boundsY;
                else
                {
                    Debug.LogError("Smth unpredictable happened. Point is inside of bounds");
                    return;
                }
            }
            coordinates = newCoordinates;
            mono.UpdateCoordinates(coordinates);
        }
        
        public void LookAt(Vector2 lookPos)
        {
            var targetDirection = (lookPos - coordinates).normalized;
            lookDirection = Vector2.Lerp(lookDirection, targetDirection, rotationAcceleration).normalized;
            rotationAngle = Vector2.SignedAngle(Vector2.up, lookDirection);
            mono.UpdateRotation(rotationAngle);
        }
        
        public void TryFireAttack1() => weaponModule1.TryFire(coordinates, rotationAngle, lookDirection);
        public void TryFireAttack2() => weaponModule2.TryFire(coordinates, rotationAngle, lookDirection);

        public void Hit(EHitTypes hitTypes)
        {
            mono.gameObject.SetActive(false);
            Died();
        }
    }
}
