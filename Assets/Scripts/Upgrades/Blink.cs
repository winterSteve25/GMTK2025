using Player;
using PrimeTween;
using UnityEngine;
using Zones;

namespace Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Blink", fileName = "Blink")]
    public class Blink : Upgrade
    {
        public override void Apply()
        {
            PlayerAbilities.Current.AddAbility(new BlinkAbility(Icon));
        }

        private class BlinkAbility : IAbility
        {
            public BlinkAbility(Sprite icon)
            {
                Icon = icon;
            }

            public float Cooldown => 10;
            public Sprite Icon { get; private set; }

            public void Trigger()
            {
                var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Tween.Scale(PlayerMovement.Current.transform, 0, 0.15f)
                    .OnComplete(() =>
                    {
                        PlayerMovement.Current.transform.position = mp;
                        Tween.Scale(PlayerMovement.Current.transform, 1, 0.15f);
                        ZoneManager.Current.CreateZone(mp, 12);
                    });
            }
        }
    }
}