using BepInEx.Configuration;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_PARKOUR = "Duct Parkour";
    private const string NAME_SHORT_PARKOUR = "Parkour";
    private const int ID_PARKOUR = 3;

    public static ConfigCacheDiskEffect<float> ParkourInteractRange;
    public static ConfigCacheDiskEffect<float> ParkourTransitionSpeed;
    public static ConfigCacheDiskEffect<bool> ParkourAutoClose;

    private static ConfigEntry<float> _parkourInteractRangeBase;
    private static ConfigEntry<float> _parkourInteractRangeFirst;
    private static ConfigEntry<float> _parkourInteractRangeSecond;

    private static ConfigEntry<float> _parkourTransitionSpeedBase;
    private static ConfigEntry<float> _parkourTransitionSpeedFirst;
    private static ConfigEntry<float> _parkourTransitionSpeedSecond;

    private static ConfigEntry<bool> _parkourAutoCloseBase;
    private static ConfigEntry<bool> _parkourAutoCloseFirst;
    private static ConfigEntry<bool> _parkourAutoCloseSecond;

    private static void InitializeParkour(ConfigFile config)
    {
        _parkourInteractRangeBase = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Base Level)", 0f,
                                               new ConfigDescription($"How much further you can reach vents with the base level of {NAME_SHORT_PARKOUR}."));
        
        _parkourInteractRangeFirst = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Added Interaction Range (First Upgrade)", 1f,
                                                new ConfigDescription($"How much further you can reach vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        _parkourInteractRangeSecond = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Second Upgrade)", 1f,
                                                 new ConfigDescription($"How much further you can reach vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        _parkourTransitionSpeedBase = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Base Level)", 0.8f,
                                                 new ConfigDescription($"A multiplier on the speed you enter and exit vents with the base level of {NAME_SHORT_PARKOUR}."));

        _parkourTransitionSpeedFirst = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (First Upgrade)", 0.6f,
                                                  new ConfigDescription($"A multiplier on the speed you enter and exit vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        _parkourTransitionSpeedSecond = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Second Upgrade)", 0.6f,
                                                   new ConfigDescription($"A multiplier on the speed you enter and exit vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        _parkourAutoCloseBase = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Base Level)", false,
                                           new ConfigDescription($"Whether the base level of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        _parkourAutoCloseFirst = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Auto Close Vents (First Upgrade)", false,
                                            new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        _parkourAutoCloseSecond = config.Bind($"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Second Upgrade)", true,
                                             new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
    }
    
    private static void CacheParkour()
    {
        ParkourInteractRange = new ConfigCacheDiskEffect<float>(0f,
                                                      (level, oldValue, newValue) => $"You can now reach vents {newValue}m further away.",
                                                      _parkourInteractRangeBase, _parkourInteractRangeFirst, _parkourInteractRangeSecond);
        
        ParkourTransitionSpeed = new ConfigCacheDiskEffect<float>(1f,
                                                        (level, oldValue, newValue) => $"You get in and out of vents {Utilities.MultiplierForDescription(newValue, "faster", "slower", out string description)}% {description}.",
                                                        _parkourTransitionSpeedBase, _parkourTransitionSpeedFirst, _parkourTransitionSpeedSecond);
        
        ParkourAutoClose = new ConfigCacheDiskEffect<bool>(false,
                                                 (level, oldValue, newValue) => $"Vents {(newValue ? "now" : "no longer")} automatically close when entering or exiting them.",
                                                 _parkourAutoCloseBase, _parkourAutoCloseFirst, _parkourAutoCloseSecond);
    }

    private static void ResetParkour()
    {
        _parkourInteractRangeBase.Value = (float)_parkourInteractRangeBase.DefaultValue;
        _parkourInteractRangeFirst.Value = (float)_parkourInteractRangeFirst.DefaultValue;
        _parkourInteractRangeSecond.Value = (float)_parkourInteractRangeSecond.DefaultValue;
        _parkourTransitionSpeedBase.Value = (float)_parkourTransitionSpeedBase.DefaultValue;
        _parkourTransitionSpeedFirst.Value = (float)_parkourTransitionSpeedFirst.DefaultValue;
        _parkourTransitionSpeedSecond.Value = (float)_parkourTransitionSpeedSecond.DefaultValue;
        _parkourAutoCloseBase.Value = (bool)_parkourAutoCloseBase.DefaultValue;
        _parkourAutoCloseFirst.Value = (bool)_parkourAutoCloseFirst.DefaultValue;
        _parkourAutoCloseSecond.Value = (bool)_parkourAutoCloseSecond.DefaultValue;
    }
}