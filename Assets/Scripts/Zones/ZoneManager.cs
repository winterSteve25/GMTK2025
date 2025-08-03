using System.Collections.Generic;
using System.Linq;
using PrimeTween;
using UnityEngine;
using Utils;

namespace Zones
{
    public class ZoneManager : MonoBehaviour
    {
        public static ZoneManager Current { get; private set; }

        public float zoneDeletionMultiplier = 1;
        public float zoneAdditionMultiplier = 1;
        
        [SerializeField] private List<Zone> zones;
        [SerializeField] private Zone prefab;

        private void Awake()
        {
            Current = this;
        }

        private void Update()
        {
            for (var i = 0; i < zones.Count; i++)
            {
                var zone = zones[i];
                zone.Radius -= (Mathf.Log(TimeKeeper.Current.GameElapsedTime / 40 + 1) + 0.5f) * Time.deltaTime * zoneDeletionMultiplier;

                if (zone.Radius <= 0f)
                {
                    zones.Remove(zone);
                    i--;
                }
            }
        }

        public void AddZoneRadius(float radius)
        {
            foreach (var zone in zones)
            {
                Tween.Custom(zone.Radius, zone.Radius + radius * zoneAdditionMultiplier, 0.2f, x => zone.Radius = x);
            }
        }

        public void CreateZone(Vector2 position, float radius)
        {
            var zone = Instantiate(prefab, transform);
            zone.transform.position = position;
            zone.Radius = radius;
            zones.Add(zone);
        }

        public bool InAnyZone(Vector2 pos)
        {
            return zones.Any(zone => zone.InZone(pos));
        }
    }
}