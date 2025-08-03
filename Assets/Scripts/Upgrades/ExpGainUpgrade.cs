using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Exp Gain", fileName = "Exp Gain")]
    public class ExpGainUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerExp.Current.experienceGainMultiplier *= 1.1f;
        }
    }
}