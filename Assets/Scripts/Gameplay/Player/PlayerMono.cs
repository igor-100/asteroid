using System;
using Gameplay.Trigger;
using UnityEngine;
namespace Asteroid.Gameplay.Player
{
    public class PlayerMono : MonoBehaviour
    {
        [SerializeField]
        private TriggerCollider triggerCollider;

        public event Action<Collider2D, IHittable> Collided = (col, hittable) => { };

        public void Init(IHittable hittable)
        {
            triggerCollider.Hittable = hittable;
            triggerCollider.EventEntered += TriggerColliderOnEventEntered;
        }

        public void UpdateCoordinates(Vector2 newPosition)
        {
            transform.position = newPosition;
        }

        private void TriggerColliderOnEventEntered(Collider2D col, IHittable hittable)
        {
            Collided(col, hittable);
        }

        private void OnDisable()
        {
            triggerCollider.EventEntered -= TriggerColliderOnEventEntered;
        }

        public void UpdateRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
