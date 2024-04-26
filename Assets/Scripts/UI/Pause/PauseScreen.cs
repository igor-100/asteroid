using System;
using Asteroid.UI.Controller;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
namespace UI.Pause
{
    public class PauseScreen : MonoBehaviour, IPauseScreen
    {
        private bool gameIsPaused = false;

        private IPauseScreenView view;
        private ISceneLoader sceneLoader;
        private UniTaskCompletionSource resumeTaskCompletionSource;

        public event Action Paused = () => { };
        public event Action Unpaused = () => { };
        public event Action BeforeRestartHappened = () => { };

        public void Init(IUiPlayerInput uiPlayerInput, ISceneLoader sceneLoader, IViewFactory viewFactory)
        {
            this.sceneLoader = sceneLoader;

            view = viewFactory.CreatePauseScreen();

            view.ResumeClicked += OnResumeClicked;
            view.RestartClicked += OnRestartClicked;
            uiPlayerInput.EscPerformed += OnEscape;
            view.Hide();
        }
        
        public void SetScoreValue(int value)
        {
            view.SetScoreValue(value);
        }
        
        public void SetRestartActive(bool isActive)
        {
            view.SetRestartActive(isActive);
        }
        public void WaitForResume(UniTaskCompletionSource tsc)
        {
            this.resumeTaskCompletionSource = tsc;
        }

        private void OnEscape()
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        private void OnResumeClicked()
        {
            Resume();
        }

        private void OnRestartClicked()
        {
            BeforeRestartHappened();
            ToNormalSpeed();
            sceneLoader.RestartScene();
        }

        public void Hide()
        {
            view.Hide();
        }

        public void Show()
        {
            view.Show();
        }

        private void Pause()
        {
            Show();
            Time.timeScale = 0f;
            gameIsPaused = true;
            Paused();
        }

        private void Resume()
        {
            Hide();
            ToNormalSpeed();
            gameIsPaused = false;
            Unpaused();
            resumeTaskCompletionSource?.TrySetResult();
        }

        private static void ToNormalSpeed()
        {
            Time.timeScale = 1f;
        }
    }
}
