using System;
using Asteroid.Configurations.ResourceEnums;
using Asteroid.Gameplay.Player;
using Gameplay.Pool;
using UnityEngine;
namespace Gameplay.Level.Enemies
{
    public interface IAsteroidEnemy : IHittable, IPoolable
    {
        event Action<IAsteroidEnemy, bool> Destroyed;
        
        void Init(EEnemies eEnemies, Vector2 coordinates, float speed, float rotationAgle, Vector2 movementDirection);
    }
}
