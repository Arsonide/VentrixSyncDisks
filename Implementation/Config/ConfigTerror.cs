using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_TERROR = "Tunnel Terror";
    private const string NAME_SHORT_TERROR = "Terror";
    private const int ID_TERROR = 8;
    
    private static ConfigEntry<int> TerrorFreakoutDurationBase;
    private static ConfigEntry<int> TerrorFreakoutDurationFirst;
    private static ConfigEntry<int> TerrorFreakoutDurationSecond;
    public static ConfigCache<int> TerrorFreakoutDuration;

    private static ConfigEntry<bool> TerrorToxicImmunityBase;
    private static ConfigEntry<bool> TerrorToxicImmunityFirst;
    private static ConfigEntry<bool> TerrorToxicImmunitySecond;
    public static ConfigCache<bool> TerrorToxicImmunity;

    private static void InitializeTerror(ConfigFile config)
    {
        TerrorFreakoutDurationBase = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (Base Level)", 4,
                                                 new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the base level of {NAME_SHORT_TERROR}."));

        TerrorFreakoutDurationFirst = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (First Upgrade)", 8,
                                            new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the first upgrade of {NAME_SHORT_TERROR}."));

        TerrorFreakoutDurationSecond = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Freakout Duration (Second Upgrade)", 12,
                                             new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the second upgrade of {NAME_SHORT_TERROR}."));
        
        TerrorToxicImmunityBase = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        TerrorToxicImmunityFirst = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        TerrorToxicImmunitySecond = config.Bind($"{ID_TERROR}. {NAME_SHORT_TERROR}", "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
    }
    
    private static void SetupTerrorCaches()
    {
        TerrorFreakoutDuration = new ConfigCache<int>(1,
                                                      (level, oldValue, newValue) => $"Popping out of vents in private rooms makes citizens awake in that room freak out for {newValue} seconds.",
                                                      TerrorFreakoutDurationBase, TerrorFreakoutDurationFirst, TerrorFreakoutDurationSecond);
        
        TerrorToxicImmunity = new ConfigCache<bool>(false,
                                                    (level, oldValue, newValue) => $"You are {(newValue ? "no longer" : "now")} affected by toxic gas in vents.",
                                                    TerrorToxicImmunityBase, TerrorToxicImmunityFirst, TerrorToxicImmunitySecond);
    }

    private static void ResetTerror()
    {
        TerrorFreakoutDurationBase.Value = (int)TerrorFreakoutDurationBase.DefaultValue;
        TerrorFreakoutDurationFirst.Value = (int)TerrorFreakoutDurationFirst.DefaultValue;
        TerrorFreakoutDurationSecond.Value = (int)TerrorFreakoutDurationSecond.DefaultValue;
        TerrorToxicImmunityBase.Value = (bool)TerrorToxicImmunityBase.DefaultValue;
        TerrorToxicImmunityFirst.Value = (bool)TerrorToxicImmunityFirst.DefaultValue;
        TerrorToxicImmunitySecond.Value = (bool)TerrorToxicImmunitySecond.DefaultValue;
    }
}