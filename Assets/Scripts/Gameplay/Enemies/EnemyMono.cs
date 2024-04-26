using System;
using Asteroid.Gameplay.Player;
using Gameplay.Trigger;
using UnityEngine;
namespace Gameplay.Level.Enemies
{
    public class EnemyMono : MonoBehaviour
    {
        [SerializeField]
        private TriggerCollider triggerCollider;
        
        private float speed;

        public event Action<Collider2D, IHittable> Collided = (col, hittable) => { };

        public void Init(Vector2 coordinates, float angle, IHittable hittable)
        {
            UpdateCoordinates(coordinates);
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
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
    }
}
