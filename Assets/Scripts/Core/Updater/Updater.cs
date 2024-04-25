using System;
using UnityEngine;
namespace Asteroid.Core.Updater
{
    public class Updater : MonoBehaviour, IUpdater
    {
        public event Action<float> Updated = (deltaTime) => { };
        public event Action Destroyed = () => { };
        
        private void Update()
        {
            Updated(Time.deltaTime);
        }

        private void OnDestroy()
        {
            Destroyed();
        }
    }
}
