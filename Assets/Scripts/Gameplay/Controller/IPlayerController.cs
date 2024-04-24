﻿using Core.Camera;
using Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public interface IPlayerController
    {
        bool IsEnabled { get; set; }
        void SetPlayer(IPlayer player);
        void SetCamera(Camera gameCamera);
    }
}