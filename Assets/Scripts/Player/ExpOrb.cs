using UnityEngine;
using Utils;

namespace Player
{
    public class ExpOrb : MonoBehaviour
    {
        [SerializeField] private AudioClip collectSound;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (!other.TryGetComponent(out PlayerExp exp)) return;
            exp.AddExp(1);
            ExpOrbManager.Current.Release(this);
            AudioPlayer.PlaySFX(collectSound);
        }
    }
}