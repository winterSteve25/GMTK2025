using UnityEngine;

namespace Upgrades
{
    public interface IAbility
    {
        float Cooldown { get; }
        Sprite Icon { get; }
        
        void Trigger();
    }
}