using HarmonyLib;
using VentVigilante.Implementation.Config;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(InteractableController), "Setup")]
public class VentInteractionRangeHook
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