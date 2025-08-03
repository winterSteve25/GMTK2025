using PrimeTween;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player
{
    public class PlayerHpComponent : HpComponent
    {
        public static PlayerHpComponent Current { get; private set; }

        [SerializeField] private Image hpSlider;
        [SerializeField] private AudioClip hurtSound;

        private void Awake()
        {
            Current = this;
        }

        protected override void OnHit(float damage)
        {
            var perc = (float)hp / maxHp;
            Tween.UIFillAmount(hpSlider, perc, 0.1f);
            CameraEffects.Current.trauma += 0.6f;
            AudioPlayer.PlaySFX(hurtSound);
        }

        protected override void OnHeal(float amount)
        {
            var perc = hp / maxHp;
            Tween.UIFillAmount(hpSlider, perc, 0.1f);
        }

        protected override void Die()
        {
            Tween.Custom(Time.timeScale, 0, 1f, x => Time.timeScale = x, useUnscaledTime: true, ease: Ease.InCirc);
            Tween.Custom(CameraEffects.Current.ChromaticAberration, 0, 0.5f, x => CameraEffects.Current.ChromaticAberration = x, useUnscaledTime: true);
            DeathUI.Current.Open();
        }
    }
}