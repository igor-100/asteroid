using System;
using UnityEngine;
namespace Gameplay.Trigger
{
    public class TriggerCollider : MonoBehaviour
    {
        public bool IsEnterable { get; set; } = true;
        
        public event Action<Collider2D> EventEntered = (col) => { };

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsEnterable)
            {
                EventEntered(collision);
            }
        }
    }
}
