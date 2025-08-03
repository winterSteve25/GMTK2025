using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyHpComponent))]
    public class Enemy : MonoBehaviour
    {
        protected EnemyHpComponent Hp;

        protected virtual void Start()
        {
            Hp = GetComponent<EnemyHpComponent>();
        }
    }
}