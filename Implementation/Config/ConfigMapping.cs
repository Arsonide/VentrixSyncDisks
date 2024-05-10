using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_MAPPING = "Acoustic Mapping";
    private const string NAME_SHORT_MAPPING = "Mapping";
    private const int ID_MAPPING = 4;
    
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

    private static void InitializeMapping(ConfigFile config)
    {
        _mappingEcholocationRangeBase = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (Base Level)", 10,
                                                   new ConfigDescription($"How far your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationRangeFirst = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (First Upgrade)", 20,
                                                    new ConfigDescription($"How far your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationRangeSecond = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (Second Upgrade)", 20,
                                                     new ConfigDescription($"How far your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSpeedBase = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (Base Level)", 0.1f,
                                                   new ConfigDescription($"How quickly your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSpeedFirst = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (First Upgrade)", 0.1f,
                                                    new ConfigDescription($"How quickly your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationSpeedSecond = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (Second Upgrade)", 0.1f,
                                                     new ConfigDescription($"How quickly your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationDurationBase = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (Base Level)", 1f,
                                                      new ConfigDescription($"How long it takes for your echolocation pulse to expire with the base level of {NAME_SHORT_MAPPING}."));
        
        _mappingEcholocationDurationFirst = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (First Upgrade)", 1f,
                                                       new ConfigDescription($"How long it takes for your echolocation pulse to expire with the first upgrade of {NAME_SHORT_MAPPING}."));

        _mappingEcholocationDurationSecond = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (Second Upgrade)", 1f,
                                                        new ConfigDescription($"How long it takes for your echolocation pulse to expire with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingCoinMultiplierBase = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (Base Level)", 1f,
                                                new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the base level of {NAME_SHORT_MAPPING}."));
        
        _mappingCoinMultiplierFirst = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (First Upgrade)", 1f,
                                                 new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        _mappingCoinMultiplierSecond = config.Bind($"{ID_MAPPING}. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (Second Upgrade)", 0.1f,
                                                  new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the second upgrade of {NAME_SHORT_MAPPING}."));
    }
    
    private static void CacheMapping()
    {
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