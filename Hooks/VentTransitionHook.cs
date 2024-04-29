using HarmonyLib;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Config;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(Player), "TransformPlayerController")]
public class VentTransitionHook
{
    private static PlayerTransitionPreset EnterPreset;
    private static PlayerTransitionPreset ExitPreset;

    private static float EnterSpeed = 1f;
    private static float ExitSpeed = 1f;
    
    [HarmonyPrefix]
    private static bool Prefix(Player __instance, PlayerTransitionPreset newEnterTransition)
    {
        if (newEnterTransition == null)
        {
            return true;
        }
        
        if (EnterPreset == null && newEnterTransition.presetName.StartsWith("VentEnter"))
        {
            EnterPreset = newEnterTransition;
            EnterSpeed = EnterPreset.transitionTime;
        }
        else if (ExitPreset == null && newEnterTransition.presetName.StartsWith("VentExit"))
        {
            ExitPreset = newEnterTransition;
            ExitSpeed = ExitPreset.transitionTime;
        }

        if (newEnterTransition == EnterPreset)
        {
            float multiplier = VentrixConfig.ParkourTransitionSpeed.GetLevel(DiskRegistry.ParkourDisk.Level);
            EnterPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? EnterSpeed * multiplier : EnterSpeed;
        }

        if (newEnterTransition == ExitPreset)
        {
            float multiplier = VentrixConfig.ParkourTransitionSpeed.GetLevel(DiskRegistry.ParkourDisk.Level);
            ExitPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? ExitSpeed * multiplier : ExitSpeed;
        }

        return true;
    }
}