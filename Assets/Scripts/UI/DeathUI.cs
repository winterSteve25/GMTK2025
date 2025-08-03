using EasyTransition;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI
{
    public class DeathUI : MonoBehaviour
    {
        public static DeathUI Current { get; private set; }
        
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TransitionSettings settings;

        private void Awake()
        {
            Current = this;
        }

        public void Open()
        {
            PlayerInput.Disabled = true;
            group.interactable = true;
            group.blocksRaycasts = true;
            Tween.Alpha(group, 1, 0.2f, useUnscaledTime: true);
        }

        public void Retry()
        {
            PlayerInput.Disabled = false;
            Tween.StopAll();
            Time.timeScale = 1;
            SceneManager.LoadScene("Game");
        }

        public void Leave()
        {
            PlayerInput.Disabled = false;
            Tween.StopAll();
            Time.timeScale = 1;
            TransitionManager.Instance().Transition("MainMenu", settings, 0);
        }
    }
}