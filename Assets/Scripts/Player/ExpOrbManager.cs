using System;
using Bullets;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.Pool;

namespace Player
{
    public class ExpOrbManager : MonoBehaviour
    {
        public static ExpOrbManager Current { get; private set; }

        [SerializeField] private ExpOrb prefab;
        private IObjectPool<ExpOrb> _pool;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            _pool = new ObjectPool<ExpOrb>(
                () => Instantiate(prefab, transform),
                x => x.gameObject.SetActive(true),
                x => x.gameObject.SetActive(false),
                x => Destroy(x.gameObject),
                true,
                40
            );
        }

        public ExpOrb GetOrb()
        {
            return _pool.Get();
        }

        public void Release(ExpOrb bullet)
        {
            _pool.Release(bullet);
        }
    }
}