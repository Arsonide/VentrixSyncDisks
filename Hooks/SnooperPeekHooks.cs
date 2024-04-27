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
}