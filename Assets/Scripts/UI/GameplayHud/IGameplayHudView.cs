using UI;
namespace Asteroid.UI.GameplayHud
{
    public interface IGameplayHudView : IView
    {
        void SetCoordinates(string text);
        void SetRotation(string text);
        void SetSpeed(string text);
        void SetLaserChargesCount(string text);
        void SetLaserReloadTime(string text);
    }
}
