using System;
using Core;
using Gameplay.Pool;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public interface IProjectile : IPoolable
    {
        event Action<IProjectile> Collided;
        
        void Init(Vector2 coordinates, float speed, float angle, IResourceManager resourceManager);
    }
}
