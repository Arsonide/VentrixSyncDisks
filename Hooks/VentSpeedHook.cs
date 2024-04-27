using HarmonyLib;
using UnityEngine;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(CharacterController), "Move")]
public class VentSpeedHook
{
    [HarmonyPrefix]
    private static bool Prefix(ref Vector3 motion)
    {
        if (!Player.Instance.inAirVent)
        {
            return true;
        }
        
        switch (DiskRegistry.RunnerDisk.Level)
        {
            case 1:
                motion *= DiskRegistry.MECHANIC_MULTIPLIER_1;
                break;
            case 2:
                motion *= DiskRegistry.MECHANIC_MULTIPLIER_2;
                break;
            case 3:
                motion *= DiskRegistry.MECHANIC_MULTIPLIER_3;
                break;
        }

        return true;
    }
}