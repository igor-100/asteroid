using UI.Pause;
namespace UI
{
    public interface IViewFactory
    {
        IPauseScreenView CreatePauseScreen();
    }
}
