using System;
using UnityEngine;
namespace Asteroid.Core.Updater
{
    public interface IUpdater
    {
        event Action<float> Updated;
        event Action Destroyed;
    }
}
