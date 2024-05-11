using BepInEx.Configuration;
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
        string section = $"{ID_PARKOUR}. {NAME_SHORT_PARKOUR}";
        
        const string ADDED_INTERACTION_RANGE_TITLE = "Added Interaction Range";
        const string ADDED_INTERACTION_RANGE_DESCRIPTION = "How much further you can reach vents";
        
        _parkourInteractRangeBase = config.Bind(section, $"{ADDED_INTERACTION_RANGE_TITLE} {LEVEL_1_TITLE}", 0f,
                                               new ConfigDescription($"{ADDED_INTERACTION_RANGE_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        _parkourInteractRangeFirst = config.Bind(section, $"{ADDED_INTERACTION_RANGE_TITLE} {LEVEL_2_TITLE}", 1f,
                                                new ConfigDescription($"{ADDED_INTERACTION_RANGE_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        _parkourInteractRangeSecond = config.Bind(section, $"{ADDED_INTERACTION_RANGE_TITLE} {LEVEL_3_TITLE}", 1f,
                                                 new ConfigDescription($"{ADDED_INTERACTION_RANGE_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        const string TRANSITION_SPEED_MULTIPLIER_TITLE = "Transition Speed Multiplier";
        const string TRANSITION_SPEED_MULTIPLIER_DESCRIPTION = "A multiplier on the speed you enter and exit vents";
        
        _parkourTransitionSpeedBase = config.Bind(section, $"{TRANSITION_SPEED_MULTIPLIER_TITLE} {LEVEL_1_TITLE}", 0.8f,
                                                 new ConfigDescription($"{TRANSITION_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_PARKOUR}."));

        _parkourTransitionSpeedFirst = config.Bind(section, $"{TRANSITION_SPEED_MULTIPLIER_TITLE} {LEVEL_2_TITLE}", 0.6f,
                                                  new ConfigDescription($"{TRANSITION_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        _parkourTransitionSpeedSecond = config.Bind(section, $"{TRANSITION_SPEED_MULTIPLIER_TITLE} {LEVEL_3_TITLE}", 0.6f,
                                                   new ConfigDescription($"{TRANSITION_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        const string AUTO_CLOSE_VENTS_TITLE = "Auto Close Vents";
        const string AUTO_CLOSE_VENTS_DESCRIPTION = "Whether vents close behind you automatically when you enter or exit them";
        
        _parkourAutoCloseBase = config.Bind(section, $"{AUTO_CLOSE_VENTS_TITLE} {LEVEL_1_TITLE}", false,
                                           new ConfigDescription($"{AUTO_CLOSE_VENTS_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        _parkourAutoCloseFirst = config.Bind(section, $"{AUTO_CLOSE_VENTS_TITLE} {LEVEL_2_TITLE}", false,
                                             new ConfigDescription($"{AUTO_CLOSE_VENTS_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
        
        _parkourAutoCloseSecond = config.Bind(section, $"{AUTO_CLOSE_VENTS_TITLE} {LEVEL_3_TITLE}", true,
                                              new ConfigDescription($"{AUTO_CLOSE_VENTS_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_PARKOUR}."));
    }
    
    private static void CacheParkour()
    {
        ParkourInteractRange = new ConfigCacheDiskEffect<float>(0f,
                                                      (level, oldValue, newValue) => $"You can now reach vents {newValue}m further away.",
                                                      _parkourInteractRangeBase, _parkourInteractRangeFirst, _parkourInteractRangeSecond);
        
        ParkourTransitionSpeed = new ConfigCacheDiskEffect<float>(1f,
                                                        (level, oldValue, newValue) => $"You get in and out of vents {Utilities.DirectMultiplierDescription(newValue, "faster", "slower", out string description)}% {description}.",
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