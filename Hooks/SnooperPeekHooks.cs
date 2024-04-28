using HarmonyLib;
using VentVigilante.Implementation.Snooping;

namespace VentVigilante.Hooks;

public class SnooperPeekHooks
{
    [HarmonyPatch(typeof(Player), "OnDuctSectionChange")]
    public class PlayerOnDuctSectionChangeHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(OutlineController), "SetOutlineActive")]
    public class OutlineControllerSetOutlineActiveHook
    {
        [HarmonyPostfix]
        private static void Postfix(OutlineController __instance, bool val)
        {
            SnoopHighlighter.RefreshActorOutline(__instance.actor);
        }
    }
    
    [HarmonyPatch(typeof(OutlineController), "SetAlpha")]
    public class OutlineControllerSetOutlineAlphaHook
    {
        [HarmonyPostfix]
        private static void Postfix(OutlineController __instance, float val)
        {
            SnoopHighlighter.RefreshActorOutline(__instance.actor);
        }
    }
    
    [HarmonyPatch(typeof(Player), "EnterVent")]
    public class PlayerEnterVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
    
    [HarmonyPatch(typeof(Player), "ExitVent")]
    public class PlayerExitVentHook
    {
        [HarmonyPostfix]
        private static void Postfix(Player __instance, bool restoreTransform = false)
        {
            SnoopHighlighter.RefreshSnoopingState();
        }
    }
}