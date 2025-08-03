using System;
using UnityEngine;
using Zones;

namespace Player
{
    [RequireComponent(typeof(PlayerHpComponent))]
    public class PlayerZoneDamage : MonoBehaviour
    {
        public static PlayerZoneDamage Current { get; private set; }
        
        [SerializeField] private float damageCooldown;
        [SerializeField] private int damage;

        private PlayerHpComponent _hpComponent;
        private float _cooldown;
        private bool _wasInZone;
        
        public float DamageCooldown => damageCooldown;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            _hpComponent = GetComponent<PlayerHpComponent>();
        }

        private void Update()
        {
            if (Zone.PlayerInZone && !_wasInZone)
            {
                _cooldown = 0;
            }

            _wasInZone = Zone.PlayerInZone;

            if (!_wasInZone)
            {
                _cooldown += Time.deltaTime;
                if (_cooldown >= damageCooldown)
                {
                    _cooldown = 0;
                    _hpComponent.Hit(damage);
                }
            }
        }
    }
}