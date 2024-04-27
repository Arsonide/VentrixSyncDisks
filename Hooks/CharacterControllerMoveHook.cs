using HarmonyLib;
using UnityEngine;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(CharacterController), "Move")]
public class CharacterControllerMoveHook
{
    [HarmonyPrefix]
    private static bool Prefix(ref Vector3 motion)
    {
        if (!Player.Instance.inAirVent)
        {
            return true;
        }
        
        switch (VentrixDiskManager.RunnerDisk.Level)
        {
            case 1:
                motion *= VentrixDiskManager.MECHANIC_MULTIPLIER_1;
                break;
            case 2:
                motion *= VentrixDiskManager.MECHANIC_MULTIPLIER_2;
                break;
            case 3:
                motion *= VentrixDiskManager.MECHANIC_MULTIPLIER_3;
                break;
        }

        return true;
    }
}