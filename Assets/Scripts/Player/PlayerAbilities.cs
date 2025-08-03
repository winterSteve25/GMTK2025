using System;
using System.Collections.Generic;
using Enemies;
using UI;
using UnityEngine;
using Upgrades;
using Utils;

namespace Player
{
    public class PlayerAbilities : MonoBehaviour
    {
        public static PlayerAbilities Current { get; private set; }

        public event Action<Enemy> OnKilledEnemy;
        
        private List<IAbility> _abilities;
        private List<IPassive> _passives;

        [SerializeField] private AbilitySlotUI[] slots;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            _abilities = new List<IAbility>();
            _passives = new List<IPassive>();
        }

        private void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                if (!PlayerInput.KeyDown(KeyCode.Alpha1 + i)) continue;
                if (!slots[i].Ready()) continue;
                _abilities[i].Trigger();
            }
        }

        public void AddAbility(IAbility ability)
        {
            if (_abilities.Count >= 3)
            {
                _abilities[0] = ability;
            }
            
            _abilities.Add(ability);
            UpdateIcons();
        }

        private void UpdateIcons()
        {
            for (int i = 0; i < 3; i++)
            {
                slots[i].Init(i >= _abilities.Count ? null : _abilities[i]);
            }
        }

        public void AddPassive(IPassive passive)
        {
            _passives.Add(passive);
            passive.Attach(this);
        }

        public void FireKilledEnemy(Enemy enemy)
        {
            OnKilledEnemy?.Invoke(enemy);
        }
    }
}