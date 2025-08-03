using UnityEngine;

namespace Utils
{
    public class HpComponent : MonoBehaviour
    {
        [SerializeField] protected float hp;
        [SerializeField] protected float maxHp;

        public bool ShouldDie { get; protected set; }
        public float Hp => hp;

        public float MaxHp
        {
            get => maxHp;
            set
            {
                float percent = hp / maxHp;
                maxHp = value;
                var newHp = maxHp * percent;
                var difference = Mathf.Abs(newHp - hp);
                if (newHp > hp)
                {
                    hp = newHp;
                    OnHeal(difference);
                }
                else if (newHp < hp)
                {
                    hp = newHp;
                    OnHit(difference);
                }
            }
        }

        private void Start()
        {
            hp = maxHp;
        }

        public void Hit(float damage)
        {
            hp -= damage;

            if (hp <= 0)
            {
                ShouldDie = true;
            }

            OnHit(damage);

            if (hp <= 0)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            hp += amount;
            OnHeal(amount);
        }

        protected virtual void OnHeal(float amount)
        {
        }

        protected virtual void OnHit(float damage)
        {
        }

        protected virtual void Die()
        {
        }
    }
}