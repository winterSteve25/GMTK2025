using Player;
using UnityEngine;

namespace Enemies
{
    public class BasicEnemy : Enemy
    {
        [SerializeField] private float speed;
        [SerializeField] private float attackCd;
        [SerializeField] private int damage;
        
        private PlayerMovement _player;
        private float _timer;

        protected override void Start()
        {
            base.Start();
            _player = PlayerMovement.Current;
        }

        private void Update()
        {
            if (Hp.ShouldDie) return;
            var dir = _player.transform.position - transform.position;
            dir.z = 0;
            dir.Normalize();
            transform.position += speed * Time.deltaTime * dir;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Hp.ShouldDie) return;
            if (!other.gameObject.CompareTag("Player")) return;
            _timer = 0;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (Hp.ShouldDie) return;
            if (!other.gameObject.TryGetComponent(out PlayerHpComponent player)) return;
            
            _timer += Time.deltaTime;
            if (_timer >= attackCd)
            {
                player.Hit(damage);
                _timer = 0;
            }
        }
    }
}