using HarmonyLib;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

public class GhostHooks
{
    [HarmonyPatch(typeof(AudioController), "PlayWorldFootstep")]
    public class AudioControllerPlayWorldFootstepHook
    {
        private static System.Random _rand = new System.Random();

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

            float footstepChance = GetFootstepChance(level);

            if (_rand.NextSingle() > footstepChance)
            {
                __result = false;
                return false;
            }

            return true;
        }

        private static float GetFootstepChance(int level)
        {
            switch (level)
            {
                case 1:
                    return 0.68f;
                case 2:
                    return 0.36f;
                case 3:
                    return 0.04f;
                default:
                    return 1f;
            }
        }
    }
}