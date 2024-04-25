using Core;
using Core.ResourceEnums;
using Asteroid.Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public interface IGameplayRoot
    {
        void Init(IResourceManager resourceManager);
    }
}
