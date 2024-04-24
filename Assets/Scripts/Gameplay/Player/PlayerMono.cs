using System;
using UnityEngine;
namespace Gameplay.Player
{
    public class PlayerMono : MonoBehaviour
    {
        public void UpdateCoordinates(Vector2 newPosition)
        {
            transform.position = newPosition;
        }
        
        public void UpdateRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
