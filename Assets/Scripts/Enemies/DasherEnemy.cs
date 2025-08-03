using Player;
using PrimeTween;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Utils;

namespace Enemies
{
    public class DasherEnemy : Enemy
    {
        [Header("Attack Settings")]
        [SerializeField] private float attackInterval = 2f;
        [SerializeField] private float range = 5f;
        [SerializeField] private float maxBashLength = 3f;
        [SerializeField] private float bashSpeed = 1f;
        [SerializeField] private float chargingSpeed = 2f;

        [Header("References")] 
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D attackArea;
        [SerializeField] private AudioResource dashSound;

        private float _timeElapsed;
        private bool _attacking;
        private Vector3 _originalScale;
        private Color _originalColor;

        protected override void Start()
        {
            base.Start();
            _originalScale = spriteRenderer.transform.localScale;
            _originalColor = spriteRenderer.color;
        }

        private void FixedUpdate()
        {
            var dir = PlayerMovement.Current.transform.position - transform.position;
            var distanceSquared = dir.sqrMagnitude;
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg), 0.1f);

            if (_timeElapsed > attackInterval && distanceSquared < range * range)
            {
                _timeElapsed = 0;
                _attacking = true;

                // Scale down animation
                var scaleDown = new Vector3(_originalScale.x, _originalScale.y * 0.15f, _originalScale.z);
                Tween.Scale(spriteRenderer.transform, scaleDown, 0.75f, ease: Ease.OutCubic)
                    .OnComplete(() =>
                    {
                        Vector3 scaleUp = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
                        Tween.Scale(spriteRenderer.transform, scaleUp, 0.1f, ease: Ease.OutCubic);
                        Tween.Color(spriteRenderer, _originalColor, 0.1f, ease: Ease.OutCubic);

                        Vector3 direction = dir.normalized;
                        Vector3 bashTarget = transform.position + direction * maxBashLength;
                        AudioPlayer.PlaySFX(dashSound);

                        Tween.Position(transform, bashTarget, bashSpeed, ease: Ease.OutCubic)
                            .OnComplete(() =>
                            {
                                _attacking = false;
                            }, false);
                    }, false);

                Tween.Color(spriteRenderer, Color.red, 0.75f, Ease.OutCubic);
            }

            if (_attacking)
            {
                MoveCharacter(dir, chargingSpeed);
                return;
            }

            if (distanceSquared > (range / 1.8f) * (range / 1.8f))
            {
                MoveCharacter(dir, 1f); // Default speed
            }

            _timeElapsed += Time.fixedDeltaTime; // Assuming GameTimeScale.TimeScale equivalent is handled elsewhere
        }

        private void MoveCharacter(Vector2 dir, float speed = 1f)
        {
            transform.position += speed * Time.deltaTime * (Vector3)dir;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Hp.ShouldDie) return;
            if (!other.gameObject.TryGetComponent(out PlayerHpComponent player)) return;
            player.Hit(1);
        }
    }
}