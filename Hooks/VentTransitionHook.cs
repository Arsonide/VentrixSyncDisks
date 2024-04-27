using HarmonyLib;
using VentVigilante.Implementation.Common;
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
            EnterPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? EnterSpeed * 0.5f : EnterSpeed;
        }

        if (newEnterTransition == ExitPreset)
        {
            ExitPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? ExitSpeed * 0.5f : ExitSpeed;
        }

        return true;
    }
}