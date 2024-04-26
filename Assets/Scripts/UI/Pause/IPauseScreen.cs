using System;
using Asteroid.UI.Controller;
using Core;
using Cysharp.Threading.Tasks;
namespace UI.Pause
{
    public interface IPauseScreen : IScreen
    {
        event Action Paused;
        event Action Unpaused;
        event Action BeforeRestartHappened;
        void Init(IUiPlayerInput uiPlayerInput, ISceneLoader sceneLoader, IViewFactory viewFactory);
        void SetScoreValue(int value);
        void SetResumeActive(bool isActive);
        void WaitForRestart(UniTaskCompletionSource tsc);
    }
}
