using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause
{
    public class PauseScreenView : BaseView, IPauseScreenView
    {
        public event Action ResumeClicked = delegate{};
        public event Action RestartClicked = delegate{};

        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private TextMeshProUGUI scoreValue;

        private void Awake()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        public void OnResumeClicked()
        {
            ResumeClicked();
        }

        public void OnRestartClicked()
        {
            RestartClicked();
        }

        public void SetScoreValue(int value)
        {
            this.scoreValue.text = value.ToString();
        }

        public void SetRestartActive(bool isActive)
        {
            restartButton.gameObject.SetActive(isActive);
        }
    }
}
