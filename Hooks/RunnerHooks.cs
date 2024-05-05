// #define FAST_VENTS // Uncomment to move very fast through vents.

using HarmonyLib;
using UnityEngine;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Hooks;

public class RunnerHooks
{
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

            int level = DiskRegistry.RunnerDisk.Level;

            if (level <= 0)
            {
                return true;
            }

#pragma warning disable 0162

#if FAST_VENTS
        motion *= 10f;
        return true;
#endif

            motion *= VentrixConfig.RunnerSpeedMultiplier.GetLevel(level);
            return true;

#pragma warning restore 0162
        }
    }
}