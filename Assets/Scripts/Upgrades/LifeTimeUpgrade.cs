using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "Lifetime Upgrade", menuName = "Upgrades/Lifetime Upgrade")]
    public class LifeTimeUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerShoot.Current.bulletLifeTime *= 0.9f;
        }
    }
}