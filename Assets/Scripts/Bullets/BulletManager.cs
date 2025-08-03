using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.Pool;

namespace Bullets
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private Bullet prefab;
        
        private IObjectPool<Bullet> _bulletPool;

        private void Start()
        {
            _bulletPool = new ObjectPool<Bullet>(
                () =>
                {
                    var bullet = Instantiate(prefab, transform);
                    bullet.Create(this);
                    GameObjectInjector.InjectObject(bullet.gameObject, bullet.gameObject.scene.GetSceneContainer());
                    return bullet;
                },
                x => x.gameObject.SetActive(true),
                x => x.gameObject.SetActive(false),
                x => Destroy(x.gameObject),
                true,
                40
            );
        }

        public Bullet Get()
        {
            return _bulletPool.Get();
        }

        public void Release(Bullet bullet)
        {
            _bulletPool.Release(bullet);
        }
    }
}