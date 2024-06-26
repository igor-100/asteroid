﻿using System;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public interface IPlayer : IHittable
    {
        Vector2 Coordinates { get; }
        float Rotation { get; }
        float Speed { get; }
        
        IWeaponModule Weapon1 { get; }
        IWeaponModule Weapon2 { get; }

        event Action Died;
        
        void Init(PlayerMono playerMono);
        void SetMovementBorders(Bounds bounds);

        void IncreaseSpeed();
        void DecreaseSpeed();
        void LookAt(Vector2 lookPos);
        void TryFireAttack1();
        void TryFireAttack2();
    }
}
