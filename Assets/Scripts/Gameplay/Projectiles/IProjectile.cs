using System;
using Configurations.Properties;
using Core;
using Core.ResourceEnums;
using Gameplay.Pool;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public interface IProjectile : IPoolable
    {
        event Action<IProjectile> Finished;

        void Init(EProjectiles eProjectiles, EHitTypes hitType, Vector2 coordinates, float speed,
            float angle,
            Vector2 lookDirection, float disappearTime = 0f);
    }
}
