using HarmonyLib;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

public class MenaceHooks
{
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            if (DiskRegistry.MenaceDisk.Level <= 0)
            {
                return;
            }

            NewRoom room = Player.Instance.currentRoom;

            // Menace doesn't scare people in public places, only where they feel safe.
            if (room == null || !Player.Instance.isTrespassing)
            {
                return;
            }

            foreach (Actor actor in room.currentOccupants)
            {
                if (!actor.isPlayer && !actor.isMachine)
                {
                    actor.AddNerve(-1000, Player.Instance);
                }
            }
        }
    }
}