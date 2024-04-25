using Core;
using Core.ResourceEnums;
using Asteroid.Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public interface IGameplay
    {
        void Init(IResourceManager resourceManager);
    }
}
