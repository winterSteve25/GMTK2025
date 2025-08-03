using Enemies;
using Player;
using UnityEngine;
using Zones;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "Zone Hopper", menuName = "Upgrades/Zone Hopper")]
    public class ZoneHopper : Upgrade
    {
        public override void Apply()
        {
            ZoneManager.Current.zoneAdditionMultiplier *= 1.2f;
            ZoneManager.Current.zoneDeletionMultiplier *= 1.5f;
            PlayerAbilities.Current.AddPassive(new CreateZone());
        }

        private class CreateZone : IPassive
        {
            public void Attach(PlayerAbilities player)
            {
                player.OnKilledEnemy += PlayerOnOnKilledEnemy;
            }

            private void PlayerOnOnKilledEnemy(Enemy obj)
            {
                if (Random.value > 0.7)
                {
                    ZoneManager.Current.CreateZone(obj.transform.position, 5);
                }
            }
        }
    }
}