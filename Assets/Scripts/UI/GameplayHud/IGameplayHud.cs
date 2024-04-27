using UI;
using UnityEngine;
namespace Asteroid.UI.GameplayHud
{
    public interface IGameplayHud : IScreen
    {
        void Init(IViewFactory viewFactory);
        
        void SetCoordinates(Vector2 coordinates);
        void SetRotation(float rotationAngle);
        void SetSpeed(float speed);
        void SetLaserChargesCount(int count);
        void SetLaserReloadTime(float time);
    }
}
