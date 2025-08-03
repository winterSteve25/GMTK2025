using System;
using Reflex.Attributes;
using UnityEngine;
using Utils;
using Zones;

namespace Bullets
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Inject] private ZoneManager _zoneManager;
        
        private BulletManager _manager;
        private Vector2 _direction;
        private float _speed;
        private float _lifetime;
        private float _bulletLifetime;
        private Action<Bullet, HpComponent> _onHit;
        private bool _outside;
        private int _pierce;
        private float _damage;

        private void Update()
        {
            var movement = _speed * Time.deltaTime * _direction;
            transform.position += new Vector3(movement.x, movement.y, 0);
            _lifetime += Time.deltaTime;

            if (_outside && _zoneManager.InAnyZone(transform.position))
            {
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                _outside = false;
            }

            if (_lifetime > _bulletLifetime && gameObject.activeSelf)
            {
                _manager.Release(this);
            }
        }

        public void Create(BulletManager manager)
        {
            _manager = manager;
        }

        public void Init(Vector2 dir, float speed, float lifetime, float damage, int pierce, Action<Bullet, HpComponent> onHit = null)
        {
            _direction = dir;
            _speed = speed;
            _lifetime = 0;
            _bulletLifetime = lifetime;
            _onHit = onHit;
            _outside = true;
            _pierce = pierce;
            _damage = damage;
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
            transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x) + 90, Vector3.forward);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!gameObject.activeSelf) return;
            if (other == null) return;
            if (!other.TryGetComponent(out HpComponent hp)) return;
            
            hp.Hit(_damage);
            _onHit?.Invoke(this, hp);
            _pierce--;

            if (_pierce <= 0)
            {
                _manager.Release(this);
            }
        }
    }
}