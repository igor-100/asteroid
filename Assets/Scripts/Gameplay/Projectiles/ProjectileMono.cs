using System;
using Gameplay.Trigger;
using UnityEngine;
namespace Asteroid.Gameplay.Projectiles
{
    public class ProjectileMono : MonoBehaviour
    {
        [SerializeField]
        private TriggerCollider triggerCollider;
        
        private float speed;

        public event Action<Collider2D> Collided = (col) => { };

        public void Launch(Vector2 coordinates, float speed, float angle)
        {
            this.speed = speed;
            this.transform.position = coordinates;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);
            triggerCollider.EventEntered += TriggerColliderOnEventEntered;
        }
        
        private void TriggerColliderOnEventEntered(Collider2D col)
        {
            Collided(col);
        }

        private void Update()
        {
            transform.Translate(speed * Time.deltaTime * Vector2.up);
        }

        private void OnDisable()
        {
            triggerCollider.EventEntered -= TriggerColliderOnEventEntered;
        }

    }
}
