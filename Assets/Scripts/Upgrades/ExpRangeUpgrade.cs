using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Exp Range", fileName = "Exp Range")]
    public class ExpRangeUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerExp.Current.radius *= 1.2f;
        }
    }
}