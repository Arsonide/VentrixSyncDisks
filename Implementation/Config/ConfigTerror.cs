using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Config.Caches;

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

        const string FREAKOUT_DURATION_TITLE = "Freakout Duration";
        const string FREAKOUT_DURATION_DESCRIPTION = "The duration a citizen freaks out when you pop out of vents in private areas";
        
        _terrorFreakoutDurationBase = config.Bind(section, $"{FREAKOUT_DURATION_TITLE} {LEVEL_1_TITLE}", 10,
                                                 new ConfigDescription($"{FREAKOUT_DURATION_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationFirst = config.Bind(section, $"{FREAKOUT_DURATION_TITLE} {LEVEL_2_TITLE}", 15,
                                            new ConfigDescription($"{FREAKOUT_DURATION_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_TERROR}."));

        _terrorFreakoutDurationSecond = config.Bind(section, $"{FREAKOUT_DURATION_TITLE} {LEVEL_3_TITLE}", 20,
                                             new ConfigDescription($"{FREAKOUT_DURATION_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_TERROR}."));
        
        const string GAS_IMMUNITY_TITLE = "Toxic Immunity";
        const string GAS_IMMUNITY_DESCRIPTION = "Whether you are granted toxic gas immunity in vents";
        
        _terrorToxicImmunityBase = config.Bind(section, $"{GAS_IMMUNITY_TITLE} {LEVEL_1_TITLE}", false,
                                               new ConfigDescription($"{GAS_IMMUNITY_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_TERROR}."));
        
        _terrorToxicImmunityFirst = config.Bind(section, $"{GAS_IMMUNITY_TITLE} {LEVEL_2_TITLE}", true,
                                                new ConfigDescription($"{GAS_IMMUNITY_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_TERROR}."));
        
        _terrorToxicImmunitySecond = config.Bind(section, $"{GAS_IMMUNITY_TITLE} {LEVEL_3_TITLE}", true,
                                                 new ConfigDescription($"{GAS_IMMUNITY_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_TERROR}."));
        
        _terrorScareableCitizensResidence = config.Bind(section, "Scareable Citizens (Residence)", 999,
                                                 new ConfigDescription($"How many citizens can be scared when you pop out of vents in private residence areas at one time."));
        
        _terrorScareableCitizensWorkplace = config.Bind(section, "Scareable Citizens (Workplace)", 2,
                                                        new ConfigDescription($"How many citizens can be scared when you pop out of vents in workplace areas at one time."));
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