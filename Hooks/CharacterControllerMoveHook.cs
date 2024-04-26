using HarmonyLib;
using UnityEngine;
using VentVigilante.Implementation.Common;

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
        
        switch (SyncDisks.MechanicLevel)
        {
            case 1:
                motion *= SyncDisks.MECHANIC_MULTIPLIER_1;
                break;
            case 2:
                motion *= SyncDisks.MECHANIC_MULTIPLIER_2;
                break;
            case 3:
                motion *= SyncDisks.MECHANIC_MULTIPLIER_3;
                break;
        }

        return true;
    }
}