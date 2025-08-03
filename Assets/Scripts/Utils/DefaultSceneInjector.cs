using Bullets;
using Reflex.Core;
using UnityEngine;
using Zones;

namespace Utils
{
    public class DefaultSceneInjector : MonoBehaviour, IInstaller
    {
        [SerializeField] private Camera cam;
        [SerializeField] private BulletManager bulletManager;
        [SerializeField] private ZoneManager zoneManager;
        
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton(cam);
            containerBuilder.AddSingleton(bulletManager);
            containerBuilder.AddSingleton(zoneManager);
        }
    }
}