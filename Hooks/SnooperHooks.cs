using System;
using HarmonyLib;
using UnityEngine;
using VentrixSyncDisks.Implementation.Snooping;

namespace VentrixSyncDisks.Hooks;

public class SnooperHooks
{
    [HarmonyPatch(typeof(Player), "OnDuctSectionChange")]
    public class PlayerOnDuctSectionChangeHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance)
        {
            SnoopManager.RefreshSnoopingState();
        }
    }

    [HarmonyPatch(typeof(Player), "EnterVent")]
    public class PlayerEnterVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopManager.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopManager.RefreshSnoopingState();
        }
    }

    [HarmonyPatch(typeof(Actor), "OnRoomChange")]
    public class ActorOnRoomChangeHook
    {
        [HarmonyPostfix]
        private static void Postfix(Actor __instance)
        {
            SnoopManager.OnActorRoomChanged(__instance);
        }
    }
}