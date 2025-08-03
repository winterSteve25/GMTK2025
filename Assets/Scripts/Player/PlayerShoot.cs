using Bullets;
using Enemies;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Player
{
    public class PlayerShoot : MonoBehaviour
    {
        public static PlayerShoot Current { get; private set; }
        
        public float shootCooldown;
        public float bulletSpeed;
        public float bulletDamage = 1;
        public int bulletPierce = 1;
        public float bulletLifeTime = 5f;
        
        [SerializeField] private Transform shootFrom;
        [SerializeField] private AudioResource shootSound;

        [Inject] private BulletManager _bulletManager;
        [Inject] private Camera _camera;

        private float _time;

        private void Awake()
        {
            Current = this;
        } 

        private void Update()
        {
            _time += Time.deltaTime;
            
            if (PlayerInput.Mouse(0) && _time >= shootCooldown)
            {
                _time = 0;
                var bullet = _bulletManager.Get();
                var dir = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                dir.z = 0;
                dir.Normalize();

                bullet.Init(dir, bulletSpeed, bulletLifeTime, bulletDamage, bulletPierce, (_, target) =>
                {
                    if (target.TryGetComponent(out Enemy enemy) && target.Hp <= 0)
                    {
                        PlayerAbilities.Current.FireKilledEnemy(enemy);
                    }
                });
                
                AudioPlayer.PlaySFX(shootSound);
                bullet.transform.position = transform.position + dir * (shootFrom.position - transform.position).magnitude;
            }
        }
    }
}