using Player;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(menuName = "Upgrades/Shoot Speed", fileName = "Shoot Speed")]
    public class ShootSpeedUpgrade : Upgrade
    {
        public override void Apply()
        {
            PlayerShoot.Current.shootCooldown *= 0.8f;
            PlayerShoot.Current.bulletSpeed *= 1.2f;
            PlayerShoot.Current.bulletDamage *= 0.8f;
        }
    }
}