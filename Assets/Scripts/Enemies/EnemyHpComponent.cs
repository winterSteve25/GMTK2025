using Player;
using PrimeTween;
using UnityEngine;
using Utils;
using Zones;

namespace Enemies
{
    public class EnemyHpComponent : HpComponent
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private static readonly int Progress = Shader.PropertyToID("_Progress");
        private bool _dying = false;

        protected override void OnHit(float damage)
        {
            if (_dying) return;

            var seq = Sequence.Create()
                .Chain(Tween.MaterialProperty(spriteRenderer.material, Progress, 1, 0.05f, Ease.InQuint))
                .Chain(Tween.MaterialProperty(spriteRenderer.material, Progress, 0, 0.1f, Ease.OutQuad));

            if (ShouldDie)
            {
                _dying = true;
                seq.OnComplete(() =>
                {
                    if (hp > 0) return;

                    Destroy(gameObject);
                    var orb = ExpOrbManager.Current.GetOrb();
                    orb.transform.position = transform.position;
                    ZoneManager.Current.AddZoneRadius(1);
                });
            }
        }
    }
}