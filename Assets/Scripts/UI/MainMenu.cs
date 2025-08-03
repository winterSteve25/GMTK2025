using EasyTransition;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TransitionSettings settings;
        [SerializeField] private TMP_Text hiddenText;

        private int _count = 0;
        
        public void Endless()
        {
            TransitionManager.Instance().Transition("Game", settings, 0);
        }

        public void Options()
        {
            _count++;

            if (_count == 69)
            {
                hiddenText.gameObject.SetActive(true);
            }
        }
    }
}