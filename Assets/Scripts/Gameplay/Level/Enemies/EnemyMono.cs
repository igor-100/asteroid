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
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        private float speed;

        public event Action<Collider2D> Collided = (col) => { };

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
        
        public void UpdateVisualRotation(float angle)
        {
            triggerCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
            spriteRenderer.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        private void TriggerColliderOnEventEntered(Collider2D col, IHittable hittable)
        {
            Collided(col);
        }

        private void OnDisable()
        {
            triggerCollider.EventEntered -= TriggerColliderOnEventEntered;
        }
    }
}
