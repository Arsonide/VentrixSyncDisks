using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_MAPPING = "Acoustic Mapping";
    private const string NAME_SHORT_MAPPING = "Mapping";
    private const int ID_MAPPING = 4;
    
    public static ConfigCacheSimple<float> MappingEcholocationSoundVolume;

    public static ConfigCacheDiskEffect<int> MappingEcholocationRange;
    public static ConfigCacheDiskEffect<float> MappingEcholocationSpeed;
    public static ConfigCacheDiskEffect<float> MappingEcholocationDuration;
    public static ConfigCacheDiskEffect<float> MappingCoinMultiplier;
    
    private static ConfigEntry<int> _mappingEcholocationRangeBase;
    private static ConfigEntry<int> _mappingEcholocationRangeFirst;
    private static ConfigEntry<int> _mappingEcholocationRangeSecond;
    
    private static ConfigEntry<float> _mappingEcholocationSpeedBase;
    private static ConfigEntry<float> _mappingEcholocationSpeedFirst;
    private static ConfigEntry<float> _mappingEcholocationSpeedSecond;

    private static ConfigEntry<float> _mappingEcholocationDurationBase;
    private static ConfigEntry<float> _mappingEcholocationDurationFirst;
    private static ConfigEntry<float> _mappingEcholocationDurationSecond;

    private static ConfigEntry<float> _mappingCoinMultiplierBase;
    private static ConfigEntry<float> _mappingCoinMultiplierFirst;
    private static ConfigEntry<float> _mappingCoinMultiplierSecond;

    private static ConfigEntry<float> _mappingEcholocationSoundVolume;

    private static void InitializeMapping(ConfigFile config)
    {
        string section = $"{ID_MAPPING}. {NAME_SHORT_MAPPING}";

        const string ECHOLOCATION_RANGE_TITLE = "Echolocation Range";
        const string ECHOLOCATION_RANGE_DESCRIPTION = "How far your echolocation pulse travels down vents";
        
        _mappingEcholocationRangeBase = config.Bind(section, $"{ECHOLOCATION_RANGE_TITLE} {LEVEL_1_TITLE}", 10,
                                                   new ConfigDescription($"{ECHOLOCATION_RANGE_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationRangeFirst = config.Bind(section, $"{ECHOLOCATION_RANGE_TITLE} {LEVEL_2_TITLE}", 20,
                                                    new ConfigDescription($"{ECHOLOCATION_RANGE_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationRangeSecond = config.Bind(section, $"{ECHOLOCATION_RANGE_TITLE} {LEVEL_3_TITLE}", 20,
                                                     new ConfigDescription($"{ECHOLOCATION_RANGE_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        const string ECHOLOCATION_SPEED_TITLE = "Echolocation Speed";
        const string ECHOLOCATION_SPEED_DESCRIPTION = "How quickly your echolocation pulse travels down vents";
        
        _mappingEcholocationSpeedBase = config.Bind(section, $"{ECHOLOCATION_SPEED_TITLE} {LEVEL_1_TITLE}", 0.1f,
                                                   new ConfigDescription($"{ECHOLOCATION_SPEED_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSpeedFirst = config.Bind(section, $"{ECHOLOCATION_SPEED_TITLE} {LEVEL_2_TITLE}", 0.1f,
                                                    new ConfigDescription($"{ECHOLOCATION_SPEED_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSpeedSecond = config.Bind(section, $"{ECHOLOCATION_SPEED_TITLE} {LEVEL_3_TITLE}", 0.1f,
                                                     new ConfigDescription($"{ECHOLOCATION_SPEED_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        const string ECHOLOCATION_DURATION_TITLE = "Echolocation Duration";
        const string ECHOLOCATION_DURATION_DESCRIPTION = "How long it takes for your echolocation pulse to expire";
        
        _mappingEcholocationDurationBase = config.Bind(section, $"{ECHOLOCATION_DURATION_TITLE} {LEVEL_1_TITLE}", 1f,
                                                      new ConfigDescription($"{ECHOLOCATION_DURATION_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationDurationFirst = config.Bind(section, $"{ECHOLOCATION_DURATION_TITLE} {LEVEL_2_TITLE}", 1f,
                                                       new ConfigDescription($"{ECHOLOCATION_DURATION_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_MAPPING}."));

        _mappingEcholocationDurationSecond = config.Bind(section, $"{ECHOLOCATION_DURATION_TITLE} {LEVEL_3_TITLE}", 1f,
                                                        new ConfigDescription($"{ECHOLOCATION_DURATION_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        const string COIN_DURATION_MULTIPLIER_TITLE = "Coin Duration Multiplier";
        const string COIN_DURATION_MULTIPLIER_DESCRIPTION = "A multiplier on echolocation duration while holding a coin";
        
        _mappingCoinMultiplierBase = config.Bind(section, $"{COIN_DURATION_MULTIPLIER_TITLE} {LEVEL_1_TITLE}", 1f,
                                                new ConfigDescription($"{COIN_DURATION_MULTIPLIER_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingCoinMultiplierFirst = config.Bind(section, $"{COIN_DURATION_MULTIPLIER_TITLE} {LEVEL_2_TITLE}", 1f,
                                                 new ConfigDescription($"{COIN_DURATION_MULTIPLIER_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingCoinMultiplierSecond = config.Bind(section, $"{COIN_DURATION_MULTIPLIER_TITLE} {LEVEL_3_TITLE}", 0.1f,
                                                  new ConfigDescription($"{COIN_DURATION_MULTIPLIER_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSoundVolume = config.Bind(section, "Echolocation Sound Volume", 0.2f,
                                                 new ConfigDescription("How loud the coin sounds that play during echolocation pulses are. Set to zero to turn them off."));
    }
    
    private static void CacheMapping()
    {
        MappingEcholocationSoundVolume = new ConfigCacheSimple<float>(_mappingEcholocationSoundVolume);

        MappingEcholocationRange = new ConfigCacheDiskEffect<int>(-1,
                                                        GetEcholocationRangeDescription,
                                                        _mappingEcholocationRangeBase, _mappingEcholocationRangeFirst, _mappingEcholocationRangeSecond);

        MappingEcholocationSpeed = new ConfigCacheDiskEffect<float>(0.1f,
                                                          (level, oldValue, newValue) => $"Your \"echolocation\" pulses now travel {Utilities.MultiplierForDescription(newValue / oldValue, "slower", "faster", out string description)}% {description}.",
                                                          _mappingEcholocationSpeedBase, _mappingEcholocationSpeedFirst, _mappingEcholocationSpeedSecond);

        MappingEcholocationDuration = new ConfigCacheDiskEffect<float>(1f,
                                                             (level, oldValue, newValue) => $"Your \"echolocation\" pulses now expire {Utilities.MultiplierForDescription(newValue / oldValue, "faster", "slower", out string description)}% {description}.",
                                                             _mappingEcholocationDurationBase, _mappingEcholocationDurationFirst, _mappingEcholocationDurationSecond);

        MappingCoinMultiplier = new ConfigCacheDiskEffect<float>(1f,
                                                       (level, oldValue, newValue) =>
                                                           $"You remember your \"echolocation\" pulse {Utilities.MultiplierForDescription(newValue, "longer", "shorter", out string description)}% {description} while holding a coin.",
                                                       _mappingCoinMultiplierBase, _mappingCoinMultiplierFirst, _mappingCoinMultiplierSecond);
    }

    private static void ResetMapping()
    {
        _mappingEcholocationSoundVolume.Value = (float)_mappingEcholocationSoundVolume.DefaultValue;
        _mappingEcholocationRangeBase.Value = (int)_mappingEcholocationRangeBase.DefaultValue;
        _mappingEcholocationRangeFirst.Value = (int)_mappingEcholocationRangeFirst.DefaultValue;
        _mappingEcholocationRangeSecond.Value = (int)_mappingEcholocationRangeSecond.DefaultValue;
        _mappingEcholocationSpeedBase.Value = (float)_mappingEcholocationSpeedBase.DefaultValue;
        _mappingEcholocationSpeedFirst.Value = (float)_mappingEcholocationSpeedFirst.DefaultValue;
        _mappingEcholocationSpeedSecond.Value = (float)_mappingEcholocationSpeedSecond.DefaultValue;
        _mappingEcholocationDurationBase.Value = (float)_mappingEcholocationDurationBase.DefaultValue;
        _mappingEcholocationDurationFirst.Value = (float)_mappingEcholocationDurationFirst.DefaultValue;
        _mappingEcholocationDurationSecond.Value = (float)_mappingEcholocationDurationSecond.DefaultValue;
        _mappingCoinMultiplierBase.Value = (float)_mappingCoinMultiplierBase.DefaultValue;
        _mappingCoinMultiplierFirst.Value = (float)_mappingCoinMultiplierFirst.DefaultValue;
        _mappingCoinMultiplierSecond.Value = (float)_mappingCoinMultiplierSecond.DefaultValue;
    }

    private static string GetEcholocationRangeDescription(int level, int oldValue, int newValue)
    {
        if (level == 1)
        {
            return "Throwing coins in air ducts will create brief \"echolocation\" pulses that show the vent network through walls.";
        }

        float change = (float)newValue / (float)oldValue;
        int percent = Utilities.MultiplierForDescription(change, "smaller", "larger", out string description);

        return $"Your \"echolocation\" pulses are now {percent}% {description}.";
    }
}