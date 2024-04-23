using UnityEngine;
namespace UI
{
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        [SerializeField] private Transform MainCanvasContent;
        [SerializeField] private Transform OverlayCanvasContent;
        [SerializeField] private Transform Overlay2CanvasContent;
        [SerializeField] private Transform Overlay3CanvasContent;

        public Transform MainCanvas => MainCanvasContent;
        public Transform OverlayCanvas => OverlayCanvasContent;
        public Transform Overlay2Canvas => Overlay2CanvasContent;
        public Transform Overlay3Canvas => Overlay3CanvasContent;
    }
}
