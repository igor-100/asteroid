using UnityEngine;
namespace Core.Camera
{
    public class OrthoCamera : MonoBehaviour, IOrthoCamera
    {
        [SerializeField] private UnityEngine.Camera mainCamera;

        public UnityEngine.Camera MainCamera => mainCamera;

        public void SetSize(float size)
        {
            mainCamera.orthographicSize = size;
        }
    }
}
