using System;
using Configurations.Properties;
using Gameplay;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public class Ship : IPlayer
    {
        public Vector2 Coordinates { get; private set; }
        public float Rotation => rotationAngle;
        public float Speed => currentSpeed;

        private PlayerMono mono;
        
        private float increaseSpeedAcceleration;
        private float decreaseSpeedAcceleration;
        private float rotationAcceleration;
        private float inertiaAcceleration;
        private float maximumSpeed;

        public IWeaponModule Weapon1 { get; private set;}
        public IWeaponModule Weapon2 { get; private set;}
        
        private float currentSpeed;
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
            mono.Init(this);
            mono.gameObject.SetActive(true);
            
            var playerProps = configuration.PlayerProperties;
            Weapon1 = new WeaponModule(playerProps.Weapon1, configuration.GetWeapon(playerProps.Weapon1));
            Weapon2 = new WeaponModule(playerProps.Weapon2, configuration.GetWeapon(playerProps.Weapon2));
            
            Coordinates = Vector2.zero;
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
            var newCoordinates = Coordinates + coordinatesDelta;
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
            Coordinates = newCoordinates;
            mono.UpdateCoordinates(Coordinates);
        }
        
        public void LookAt(Vector2 lookPos)
        {
            var targetDirection = (lookPos - Coordinates).normalized;
            lookDirection = Vector2.Lerp(lookDirection, targetDirection, rotationAcceleration).normalized;
            rotationAngle = Vector2.SignedAngle(Vector2.up, lookDirection);
            mono.UpdateRotation(rotationAngle);
        }
        
        public void TryFireAttack1() => Weapon1.TryFire(Coordinates, rotationAngle, lookDirection);
        public void TryFireAttack2() => Weapon2.TryFire(Coordinates, rotationAngle, lookDirection);

        public void Hit(EHitTypes hitTypes)
        {
            mono.gameObject.SetActive(false);
            Died();
        }
    }
}
