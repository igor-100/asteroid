using System;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Gameplay.Player;
using Configurations.Properties;
using Gameplay.Pool;
using UnityEngine;
namespace Gameplay.Level.Enemies
{
    public interface IEnemy : IHittable, IPoolable
    {
        public Vector2 Coordinates { get; }
        
        EEnemies EType { get; }

        event Action<IEnemy, bool, EHitTypes> GotHit;
        
        void Init(EEnemies eEnemies, Vector2 coordinates, float speed, float rotationAngle, Vector2 movementDirection = default,
            IPlayer playerToFollow = null);
    }
}
