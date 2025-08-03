using System;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class TimeKeeper : MonoBehaviour
    {
        public static TimeKeeper Current { get; private set; }

        public float GameElapsedTime { get; private set; }
        
        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            Current = this;
        }

        private void Update()
        {
            GameElapsedTime += Time.deltaTime;
            text.text = TimeSpan.FromSeconds(GameElapsedTime).ToString(@"mm\:ss");
        }
    }
}