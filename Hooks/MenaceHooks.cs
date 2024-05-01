using HarmonyLib;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Freakouts;
using System.Linq;
using VentrixSyncDisks.Implementation.Config;

namespace VentrixSyncDisks.Hooks;

public class MenaceHooks
{
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            int level = DiskRegistry.MenaceDisk.Level;
            
            if (level <= 0)
            {
                return;
            }

            NewRoom room = Player.Instance.currentRoom;
            
            // Menace doesn't scare people in public places, only where they feel safe.
            if (room == null || room.IsAccessAllowed(Player.Instance))
            {
                return;
            }
            
            foreach (Actor actor in room.currentOccupants)
            {
                if (actor.isPlayer || actor.isMachine)
                {
                    continue;
                }
                
                // IL2CPP weirdness. We need the human to get their ID for serialization.
                Human human = ((dynamic)actor).Cast<Human>();

                if (human == null)
                {
                    continue;
                }

                // When I put on the mask, I fear no vent goblin.
                if (!human.isEnforcer || !human.isOnDuty)
                {
                    FreakoutManager.StartFreakout(human, VentrixConfig.MenaceFreakoutDuration.GetLevel(level));
                }
            }
        }
    }
        
    [HarmonyPatch(typeof(StatusController), "ToxicGas")]
    public class StatusControllerToxicGasHook
    {
        [HarmonyPrefix]
        private static bool Prefix(StatusController __instance, StatusController.StatusInstance inst)
        {
            if (DiskRegistry.MenaceDisk.Level >= 2 && Player.Instance.inAirVent)
            {
                if (Player.Instance.gasLevel > 0f)
                {
                    Player.Instance.gasLevel = 0f;
                }
                
                __instance.RemoveAllCounts(inst);
                return false;
            }

            return true;
        }
    }
}