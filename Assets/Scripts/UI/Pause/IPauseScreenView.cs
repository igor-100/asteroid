using System;
namespace UI.Pause
{
    public interface IPauseScreenView : IView
    {
        public event Action ResumeClicked;
        public event Action RestartClicked;

        void SetScoreValue(int value);
        void SetRestartActive(bool isActive);
    }
}
