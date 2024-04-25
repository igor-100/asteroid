using System;
using Core;
using Core.ResourceEnums;
using Gameplay.Pool;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public interface IProjectile : IPoolable
    {
        event Action<IProjectile> Finished;

        void Init(EProjectiles eProjectiles, Vector2 coordinates, float speed, float angle,
            float disappearTime = 0f);
    }
}
