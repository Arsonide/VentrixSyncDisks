using HarmonyLib;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(MainMenuController), "Start")]
public class MainMenuControllerHook
{
    [HarmonyPostfix]
    public static void Postfix()
    {
        // TODO - Example Hook
    }
}