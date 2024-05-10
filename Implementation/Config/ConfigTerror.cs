using BepInEx.Configuration;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_TERROR = "Tunnel Terror";
    private const string NAME_SHORT_TERROR = "Terror";
    private const int ID_TERROR = 8;
    
    public static ConfigCacheDiskEffect<int> TerrorFreakoutDuration;
    public static ConfigCacheDiskEffect<bool> TerrorToxicImmunity;

    private static ConfigEntry<int> _terrorFreakoutDurationBase;
    private static ConfigEntry<int> _terrorFreakoutDurationFirst;
    private static ConfigEntry<int> _terrorFreakoutDurationSecond;

    private static ConfigEntry<bool> _terrorToxicImmunityBase;
    private static ConfigEntry<bool> _terrorToxicImmunityFirst;
    private static ConfigEntry<bool> _terrorToxicImmunitySecond;

    private static void InitializeTerror(ConfigFile config)
    {
        _terrorFreakoutDurationBase = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (Base Level)", 4,
                                                 new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the base level of {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationFirst = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (First Upgrade)", 8,
                                            new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the first upgrade of {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationSecond = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (Second Upgrade)", 12,
                                             new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the second upgrade of {NAME_SHORT_TERROR}."));
        
        _terrorToxicImmunityBase = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        _terrorToxicImmunityFirst = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        _terrorToxicImmunitySecond = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
    }
    
    private static void CacheTerror()
    {
        TerrorFreakoutDuration = new ConfigCacheDiskEffect<int>(1,
                                                      (level, oldValue, newValue) => $"Popping out of vents in private rooms makes citizens awake in that room freak out for {newValue} seconds.",
                                                      _terrorFreakoutDurationBase, _terrorFreakoutDurationFirst, _terrorFreakoutDurationSecond);
        
        TerrorToxicImmunity = new ConfigCacheDiskEffect<bool>(false,
                                                    (level, oldValue, newValue) => $"You are {(newValue ? "no longer" : "now")} affected by toxic gas in vents.",
                                                    _terrorToxicImmunityBase, _terrorToxicImmunityFirst, _terrorToxicImmunitySecond);
    }

    private static void ResetTerror()
    {
        _terrorFreakoutDurationBase.Value = (int)_terrorFreakoutDurationBase.DefaultValue;
        _terrorFreakoutDurationFirst.Value = (int)_terrorFreakoutDurationFirst.DefaultValue;
        _terrorFreakoutDurationSecond.Value = (int)_terrorFreakoutDurationSecond.DefaultValue;
        _terrorToxicImmunityBase.Value = (bool)_terrorToxicImmunityBase.DefaultValue;
        _terrorToxicImmunityFirst.Value = (bool)_terrorToxicImmunityFirst.DefaultValue;
        _terrorToxicImmunitySecond.Value = (bool)_terrorToxicImmunitySecond.DefaultValue;
    }
}