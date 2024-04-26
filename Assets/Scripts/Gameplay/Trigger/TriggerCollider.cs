using System;
using Asteroid.Gameplay.Player;
using UnityEngine;
namespace Gameplay.Trigger
{
    public class TriggerCollider : MonoBehaviour
    {
        public bool IsEnterable { get; set; } = true;
        public IHittable Hittable { get; set; }
        
        public event Action<Collider2D, IHittable> EventEntered = (col, hittable) => { };

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsEnterable)
            {
                IHittable hittable = null;
                if (collision.TryGetComponent<TriggerCollider>(out var otherTriggerCollider))
                    hittable = otherTriggerCollider.Hittable;
                EventEntered(collision, hittable);
            }
        }
    }
}
