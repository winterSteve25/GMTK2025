using PrimeTween;
using UnityEngine;

public static class BootStrap
{
    [RuntimeInitializeOnLoadMethod]
    public static void Boot()
    {
        PrimeTweenConfig.warnEndValueEqualsCurrent = false;
    }
}