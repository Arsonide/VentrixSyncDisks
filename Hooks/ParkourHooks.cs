using System;
using HarmonyLib;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Hooks;

public class ParkourHooks
{
    [HarmonyPatch(typeof(InteractableController), "Setup")]
    public class InteractableControllerSetupHook
    {
        private static InteractablePreset _airVentPreset;
        private static float _ventRangeModifierCache = -1;

        [HarmonyPostfix]
        private static void Postfix(Interactable newInteractable)
        {
            if (newInteractable == null || newInteractable.preset == null || newInteractable.preset.name != "AirVent" || newInteractable.preset == _airVentPreset)
            {
                return;
            }

            _airVentPreset = newInteractable.preset;
            RefreshVentInteractionRanges();
        }

        public static void RefreshVentInteractionRanges()
        {
            if (_airVentPreset == null)
            {
                return;
            }

            if (_ventRangeModifierCache < 0f)
            {
                _ventRangeModifierCache = _airVentPreset.rangeModifier;
            }

            int level = DiskRegistry.ParkourDisk.Level;

            if (level > 0)
            {
                float extension = VentrixConfig.ParkourInteractRange.GetLevel(level);
                _airVentPreset.rangeModifier = _ventRangeModifierCache + extension;
            }
            else
            {
                _airVentPreset.rangeModifier = _ventRangeModifierCache;
            }
        }
    }
    
    [HarmonyPatch(typeof(Interactable), "OnInteraction", new Type[] {typeof(InteractablePreset.InteractionAction), typeof(Actor), typeof(bool), typeof(float)})]
    public class InteractableOnInteractionHook
    {
        [HarmonyPostfix]
        private static void Postfix(Interactable __instance, InteractablePreset.InteractionAction action, Actor who, bool allowDelays, float additionalDelay)
        {
            int level = DiskRegistry.ParkourDisk.Level;
        
            if (level <= 0)
            {
                return;
            }
        
            if (__instance == null || who != Player.Instance)
            {
                return;
            }

            if (__instance.preset.name == "AirVent" && action.interactionName == "Enter")
            {
                bool autoClose = VentrixConfig.ParkourAutoClose.GetLevel(level);

                if (autoClose)
                {
                    __instance.OnInteraction(InteractablePreset.InteractionKey.primary, Player.Instance);
                }
            }
        }
    }
    
    [HarmonyPatch(typeof(Player), "TransformPlayerController")]
    public class PlayerTransformPlayerControllerHook
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
                float multiplier = VentrixConfig.ParkourTransitionSpeed.GetLevel(DiskRegistry.ParkourDisk.Level);
                EnterPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? EnterSpeed * multiplier : EnterSpeed;
            }

            if (newEnterTransition == ExitPreset)
            {
                float multiplier = VentrixConfig.ParkourTransitionSpeed.GetLevel(DiskRegistry.ParkourDisk.Level);
                ExitPreset.transitionTime = DiskRegistry.ParkourDisk.Level > 0 ? ExitSpeed * multiplier : ExitSpeed;
            }

            return true;
        }
    }
}