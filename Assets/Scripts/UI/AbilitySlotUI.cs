using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace UI
{
    public class AbilitySlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private Image cdImage;
        
        private IAbility _ability;
        private float _cooldown;

        public void Init(IAbility ability)
        {
            if (ability == null)
            {
                _cooldown = 0;
                return;
            }

            if (ability == _ability)
            {
                return;
            }
            
            _ability = ability;
            _cooldown = ability.Cooldown;
            icon.sprite = ability.Icon;
            cdImage.sprite = ability.Icon;
        }

        private void Update()
        {
            if (_ability == null) return;
            _cooldown -= Time.deltaTime;
            _cooldown = Mathf.Clamp(_cooldown, 0, _ability.Cooldown);
            cdImage.fillAmount = Mathf.Lerp(cdImage.fillAmount, _cooldown / _ability.Cooldown, 1);
        }

        public bool Ready()
        {
            return _ability != null && _cooldown <= 0;
        }
    }
}