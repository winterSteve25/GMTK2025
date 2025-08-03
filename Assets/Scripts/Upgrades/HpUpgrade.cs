using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "HpUpgrade", menuName = "Upgrades/HpUpgrade")]
    public class HpUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerHpComponent.Current.MaxHp *= 1.2f;
        }
    }
}