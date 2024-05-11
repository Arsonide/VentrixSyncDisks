using System.Collections.Generic;
using HarmonyLib;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Freakouts;
using VentrixSyncDisks.Implementation.Config;

namespace VentrixSyncDisks.Hooks;

public class TerrorHooks
{
    private static readonly List<Human> _terrorTargets = new List<Human>();
    
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            int level = DiskRegistry.TerrorDisk.Level;
            
            if (level <= 0)
            {
                return;
            }

            NewRoom room = Player.Instance.currentRoom;
            
            // Terror doesn't scare people in public places, only where they feel safe.
            int scareable = GetScareablePeopleInRoom(room);
            
            if (scareable <= 0)
            {
                return;
            }
            
            _terrorTargets.Clear();
            int targetCount = 0;
            
            // Apparently freaking out can cause them to immediately disappear from their current room, so we will create a list of targets first, before freaking them out.
            foreach (Actor actor in room.currentOccupants)
            {
                if (targetCount >= scareable)
                {
                    break;
                }
                
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
                    _terrorTargets.Add(human);
                    targetCount++;
                }
            }

            foreach (Human human in _terrorTargets)
            {
                FreakoutManager.StartFreakout(human, VentrixConfig.TerrorFreakoutDuration.GetLevel(level));
            }
        }
    }
        
    [HarmonyPatch(typeof(StatusController), "ToxicGas")]
    public class StatusControllerToxicGasHook
    {
        [HarmonyPrefix]
        private static bool Prefix(StatusController __instance, StatusController.StatusInstance inst)
        {
            if (!Player.Instance.inAirVent || !VentrixConfig.TerrorToxicImmunity.GetLevel(DiskRegistry.TerrorDisk.Level))
            {
                return true;
            }

            if (Player.Instance.gasLevel > 0f)
            {
                Player.Instance.gasLevel = 0f;
            }
                
            __instance.RemoveAllCounts(inst);
            return false;

        }
    }

    private static int GetScareablePeopleInRoom(NewRoom room)
    {
        if (room == null)
        {
            return 0;
        }
        
        NewGameLocation location = room.gameLocation;
        
        if (location == null)
        {
            return 0;
        }

        if (room.IsAccessAllowed(Player.Instance) || room.gameLocation.isLobby || room.gameLocation.isOutside)
        {
            return 0;
        }
        
        return location.access == AddressPreset.AccessType.residents ? VentrixConfig.TerrorScareableCitizensResidence.Value : VentrixConfig.TerrorScareableCitizensWorkplace.Value;
    }
}