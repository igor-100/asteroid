using Core;
using Core.ResourceEnums;
using Gameplay.Player;
using UnityEngine;
namespace Gameplay
{
    public interface IGameplay
    {
        void Init(IResourceManager resourceManager);
    }
}
