using Asteroid.UI.GameplayHud;
using UI.Pause;
namespace UI
{
    public interface IViewFactory
    {
        IPauseScreenView CreatePauseScreen();
        IGameplayHudView CreateGameplayHudView();
    }
}
