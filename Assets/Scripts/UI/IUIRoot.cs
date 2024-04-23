using UnityEngine;
namespace UI
{
    public interface IUIRoot
    {
        Transform MainCanvas { get; }
        Transform OverlayCanvas { get; }
        Transform Overlay2Canvas { get; }
        Transform Overlay3Canvas { get; }
    }
}
