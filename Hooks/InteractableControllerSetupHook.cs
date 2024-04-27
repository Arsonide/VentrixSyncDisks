using HarmonyLib;
using VentVigilante.Implementation.Disks;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(InteractableController), "Setup")]
public class InteractableControllerSetupHook
{
    private static InteractablePreset _airVentPreset;
    private static float _ventRangeModifierCache = -1;
    private static float _ventRecognitionRangeCache = -1;
    
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
        
        if (_ventRecognitionRangeCache < 0f)
        {
            _ventRecognitionRangeCache = _airVentPreset.recognitionRange;
        }

        if (VentrixDiskManager.ParkourDisk.Level >= 2)
        {
            _airVentPreset.rangeModifier = _ventRangeModifierCache + 1f;
        }
        else
        {
            _airVentPreset.rangeModifier = _ventRangeModifierCache;
        }
    }
}