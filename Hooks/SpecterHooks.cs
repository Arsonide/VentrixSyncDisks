using HarmonyLib;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Hooks;

public class SpecterHooks
{
    [HarmonyPatch(typeof(AudioController), "PlayWorldFootstep")]
    public class AudioControllerPlayWorldFootstepHook
    {
        private static readonly System.Random _rand = new System.Random();

        [HarmonyPrefix]
        private static bool Prefix(AudioController __instance, ref bool __result, AudioEvent eventPreset, Actor actor, bool rightFoot = false)
        {
            if (actor == null || !actor.isPlayer || !Player.Instance.inAirVent)
            {
                return true;
            }
            
            int level = DiskRegistry.SpecterDisk.Level;
            
            if (level <= 0)
            {
                return true;
            }

            float footstepChance = VentrixConfig.SpecterFootstepChance.GetLevel(level);

            if (_rand.NextSingle() > footstepChance)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
    
    [HarmonyPatch(typeof(StatusController), "Cold")]
    public class StatusControllerColdHook
    {
        [HarmonyPrefix]
        private static bool Prefix(StatusController __instance, StatusController.StatusInstance inst)
        {
            if (!Player.Instance.inAirVent)
            {
                return true;
            }
            
            int level = DiskRegistry.SpecterDisk.Level;
            
            if (level <= 0 || !VentrixConfig.SpecterColdImmunity.GetLevel(level))
            {
                return true;
            }

            if (Player.Instance.heat < 0.5f)
            {
                Player.Instance.heat = 0.5f;
            }
                
            __instance.RemoveAllCounts(inst);
            return false;

        }
    }
}