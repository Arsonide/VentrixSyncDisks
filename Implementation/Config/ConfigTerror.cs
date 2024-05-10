﻿using BepInEx.Configuration;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_TERROR = "Tunnel Terror";
    private const string NAME_SHORT_TERROR = "Terror";
    private const int ID_TERROR = 7;
    
    public static ConfigCacheSimple<int> TerrorScareableCitizensResidence;
    public static ConfigCacheSimple<int> TerrorScareableCitizensWorkplace;
    
    public static ConfigCacheDiskEffect<int> TerrorFreakoutDuration;
    public static ConfigCacheDiskEffect<bool> TerrorToxicImmunity;

    private static ConfigEntry<int> _terrorFreakoutDurationBase;
    private static ConfigEntry<int> _terrorFreakoutDurationFirst;
    private static ConfigEntry<int> _terrorFreakoutDurationSecond;

    private static ConfigEntry<bool> _terrorToxicImmunityBase;
    private static ConfigEntry<bool> _terrorToxicImmunityFirst;
    private static ConfigEntry<bool> _terrorToxicImmunitySecond;

    private static ConfigEntry<int> _terrorScareableCitizensResidence;
    private static ConfigEntry<int> _terrorScareableCitizensWorkplace;
    
    private static void InitializeTerror(ConfigFile config)
    {
        string section = $"{ID_TERROR}. {NAME_SHORT_TERROR}";

        _terrorFreakoutDurationBase = config.Bind(section, "Freakout Duration (Base Level)", 10,
                                                 new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the base level of {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationFirst = config.Bind(section, "Freakout Duration (First Upgrade)", 15,
                                            new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the first upgrade of {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationSecond = config.Bind(section, "Freakout Duration (Second Upgrade)", 20,
                                             new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the second upgrade of {NAME_SHORT_TERROR}."));
        
        _terrorToxicImmunityBase = config.Bind(section, "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        _terrorToxicImmunityFirst = config.Bind(section, "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        _terrorToxicImmunitySecond = config.Bind(section, "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_TERROR} grants you toxic gas immunity in vents."));
        
        _terrorScareableCitizensResidence = config.Bind(section, "Scareable Citizens (Residence)", 999,
                                                 new ConfigDescription($"When scaring citizens with {NAME_SHORT_TERROR}, how many can be scared when you pop out of vents in private residence rooms."));
        
        _terrorScareableCitizensWorkplace = config.Bind(section, "Scareable Citizens (Workplace)", 2,
                                                        new ConfigDescription($"When scaring citizens with {NAME_SHORT_TERROR}, how many can be scared when you pop out of vents in private workplace rooms."));
    }
    
    private static void CacheTerror()
    {
        TerrorScareableCitizensResidence = new ConfigCacheSimple<int>(_terrorScareableCitizensResidence);
        TerrorScareableCitizensWorkplace = new ConfigCacheSimple<int>(_terrorScareableCitizensWorkplace);
        
        TerrorFreakoutDuration = new ConfigCacheDiskEffect<int>(1,
                                                      (level, oldValue, newValue) => $"Popping out of vents in private rooms makes citizens awake in that room freak out for {newValue} seconds.",
                                                      _terrorFreakoutDurationBase, _terrorFreakoutDurationFirst, _terrorFreakoutDurationSecond);
        
        TerrorToxicImmunity = new ConfigCacheDiskEffect<bool>(false,
                                                    (level, oldValue, newValue) => $"You are {(newValue ? "no longer" : "now")} affected by toxic gas in vents.",
                                                    _terrorToxicImmunityBase, _terrorToxicImmunityFirst, _terrorToxicImmunitySecond);
    }

    private static void ResetTerror()
    {
        _terrorScareableCitizensResidence.Value = (int)_terrorScareableCitizensResidence.DefaultValue;
        _terrorScareableCitizensWorkplace.Value = (int)_terrorScareableCitizensWorkplace.DefaultValue;
        _terrorFreakoutDurationBase.Value = (int)_terrorFreakoutDurationBase.DefaultValue;
        _terrorFreakoutDurationFirst.Value = (int)_terrorFreakoutDurationFirst.DefaultValue;
        _terrorFreakoutDurationSecond.Value = (int)_terrorFreakoutDurationSecond.DefaultValue;
        _terrorToxicImmunityBase.Value = (bool)_terrorToxicImmunityBase.DefaultValue;
        _terrorToxicImmunityFirst.Value = (bool)_terrorToxicImmunityFirst.DefaultValue;
        _terrorToxicImmunitySecond.Value = (bool)_terrorToxicImmunitySecond.DefaultValue;
    }
}