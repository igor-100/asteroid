using Configurations.Properties;
using UnityEngine;
namespace Gameplay.Player
{
    public class Ship : IPlayer
    {
        private float increaseSpeedAcceleration;
        private float decreaseSpeedAcceleration;
        private float rotationAcceleration;
        private float inertiaAcceleration;
        private float maximumSpeed;

        private PlayerMono mono;
        
        private float currentSpeed;
        private Vector2 coordinates;
        private Vector2 lookDirection;
        private Vector2 inertiaDirection;
        private float rotationAngle;

        public void Init(PlayerMono playerMono, PlayerProperties playerProps)
        {
            this.mono = playerMono;
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
    }
}
