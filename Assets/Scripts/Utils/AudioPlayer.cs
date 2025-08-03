using UnityEngine;
using UnityEngine.Audio;

namespace Utils
{
    public class AudioPlayer : MonoBehaviour
    {
        public static AudioPlayer Instance { get; private set; }

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip uiClick;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static void PlaySFX(AudioResource resource)
        {
            Instance.sfxSource.resource = resource;
            Instance.sfxSource.Play();
        }

        public static void PlayClick()
        {
            PlaySFX(Instance.uiClick);
        }
    }
}