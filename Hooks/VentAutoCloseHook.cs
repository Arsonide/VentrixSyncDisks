using System;
using HarmonyLib;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(Interactable), "OnInteraction", new Type[] {typeof(InteractablePreset.InteractionAction), typeof(Actor), typeof(bool), typeof(float)})]
public class VentAutoCloseHook
{
    [HarmonyPostfix]
    private static void Postfix(Interactable __instance, InteractablePreset.InteractionAction action, Actor who, bool allowDelays, float additionalDelay)
    {
        if (DiskRegistry.ParkourDisk.Level < 3)
        {
            return;
        }
        
        if (__instance == null || who != Player.Instance)
        {
            return;
        }

        if (__instance.preset.name == "AirVent" && action.interactionName == "Enter")
        {
            __instance.OnInteraction(InteractablePreset.InteractionKey.primary, Player.Instance);
        }
    }
}