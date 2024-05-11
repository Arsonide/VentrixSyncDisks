using SOD.Common;
using SOD.Common.Helpers;

namespace VentrixSyncDisks.Implementation.Common;

public static class InputManager
{
    public static bool JumpPressed { get; private set; }
    public static bool CrouchPressed { get; private set; }
    
    public static void Initialize()
    {
        Lib.InputDetection.OnButtonStateChanged -= OnButtonStateChanged;
        Lib.InputDetection.OnButtonStateChanged += OnButtonStateChanged;
    }

    public static void Uninitialize()
    {
        Lib.InputDetection.OnButtonStateChanged -= OnButtonStateChanged;
    }
    
    private static void OnButtonStateChanged(object sender, InputDetectionEventArgs args)
    {
        switch (args.Key)
        {
            case InteractablePreset.InteractionKey.jump:
                JumpPressed = args.IsDown;
                break;
            case InteractablePreset.InteractionKey.crouch:
                CrouchPressed = args.IsDown;
                break;
        }
    }
}