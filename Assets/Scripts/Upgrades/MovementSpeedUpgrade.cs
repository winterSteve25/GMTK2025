using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Movement Speed", fileName = "Movement Speed")]
    public class MovementSpeedUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerMovement.Current.moveSpeed *= 1.1f;
        }
    }
}