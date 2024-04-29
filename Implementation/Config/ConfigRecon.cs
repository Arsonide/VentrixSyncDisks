using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private static ConfigEntry<int> MappingEcholocationRangeBase;
    private static ConfigEntry<int> MappingEcholocationRangeFirst;
    private static ConfigEntry<int> MappingEcholocationRangeSecond;
    public static ConfigCache<int> MappingEcholocationRange;
    
    private static ConfigEntry<float> MappingEcholocationSpeedBase;
    private static ConfigEntry<float> MappingEcholocationSpeedFirst;
    private static ConfigEntry<float> MappingEcholocationSpeedSecond;
    public static ConfigCache<float> MappingEcholocationSpeed;

    private static ConfigEntry<float> MappingEcholocationDurationBase;
    private static ConfigEntry<float> MappingEcholocationDurationFirst;
    private static ConfigEntry<float> MappingEcholocationDurationSecond;
    public static ConfigCache<float> MappingEcholocationDuration;

    private static ConfigEntry<float> MappingCoinMultiplierBase;
    private static ConfigEntry<float> MappingCoinMultiplierFirst;
    private static ConfigEntry<float> MappingCoinMultiplierSecond;
    public static ConfigCache<float> MappingCoinMultiplier;

    public static ConfigEntry<string> SnoopingOutlineColorHex;

    private static void InitializeRecon(ConfigFile config)
    {
        MappingEcholocationRangeBase = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Range (Base Level)", 25,
                                                   new ConfigDescription($"How far your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationRangeFirst = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Range (First Upgrade)", 50,
                                                    new ConfigDescription($"How far your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationRangeSecond = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Range (Second Upgrade)", 50,
                                                     new ConfigDescription($"How far your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedBase = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Speed (Base Level)", 0.1f,
                                                   new ConfigDescription($"How quickly your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedFirst = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Speed (First Upgrade)", 0.1f,
                                                    new ConfigDescription($"How quickly your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedSecond = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Speed (Second Upgrade)", 0.1f,
                                                     new ConfigDescription($"How quickly your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationDurationBase = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Duration (Base Level)", 1f,
                                                      new ConfigDescription($"How long it takes for your echolocation pulse to expire with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationDurationFirst = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Duration (First Upgrade)", 1f,
                                                       new ConfigDescription($"How long it takes for your echolocation pulse to expire with the first upgrade of {NAME_SHORT_MAPPING}."));

        MappingEcholocationDurationSecond = config.Bind($"4. {NAME_SHORT_MAPPING}", "Echolocation Duration (Second Upgrade)", 1f,
                                                        new ConfigDescription($"How long it takes for your echolocation pulse to expire with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierBase = config.Bind($"4. {NAME_SHORT_MAPPING}", "Coin Duration Multiplier (Base Level)", 1f,
                                                new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierFirst = config.Bind($"4. {NAME_SHORT_MAPPING}", "Coin Duration Multiplier (First Upgrade)", 1f,
                                                 new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierSecond = config.Bind($"4. {NAME_SHORT_MAPPING}", "Coin Duration Multiplier (Second Upgrade)", 0.1f,
                                                  new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        SnoopingOutlineColorHex = config.Bind($"5. {NAME_SHORT_SNOOPING}", "Highlight Color Multiplier", "FFFFFF",
                                              new ConfigDescription($"A hex code for what color the {NAME_SHORT_SNOOPING} outline will be multiplied by."));

        // Setup Caches
        MappingEcholocationRange = new ConfigCache<int>(-1,
                                                        GetEcholocationRangeDescription,
                                                        MappingEcholocationRangeBase, MappingEcholocationRangeFirst, MappingEcholocationRangeSecond);

        MappingEcholocationSpeed = new ConfigCache<float>(0.1f,
                                                          (level, oldValue, newValue) => $"Your \"echolocation\" pulses now travel {Utilities.MultiplierForDescription(newValue / oldValue, "slower", "faster", out string description)}% {description}.",
                                                          MappingEcholocationSpeedBase, MappingEcholocationSpeedFirst, MappingEcholocationSpeedSecond);

        MappingEcholocationDuration = new ConfigCache<float>(1f,
                                                             (level, oldValue, newValue) => $"Your \"echolocation\" pulses now expire {Utilities.MultiplierForDescription(newValue / oldValue, "faster", "slower", out string description)}% {description}.",
                                                             MappingEcholocationDurationBase, MappingEcholocationDurationFirst, MappingEcholocationDurationSecond);

        MappingCoinMultiplier = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) =>
                                                           $"You remember your \"echolocation\" pulse {Utilities.MultiplierForDescription(newValue, "longer", "shorter", out string description)}% {description} while holding a coin.",
                                                 MappingCoinMultiplierBase, MappingCoinMultiplierFirst, MappingCoinMultiplierSecond);
    }

    public static void ResetRecon()
    {
        MappingEcholocationRangeBase.Value = (int)MappingEcholocationRangeBase.DefaultValue;
        MappingEcholocationRangeFirst.Value = (int)MappingEcholocationRangeFirst.DefaultValue;
        MappingEcholocationRangeSecond.Value = (int)MappingEcholocationRangeSecond.DefaultValue;
        MappingEcholocationSpeedBase.Value = (float)MappingEcholocationSpeedBase.DefaultValue;
        MappingEcholocationSpeedFirst.Value = (float)MappingEcholocationSpeedFirst.DefaultValue;
        MappingEcholocationSpeedSecond.Value = (float)MappingEcholocationSpeedSecond.DefaultValue;
        MappingEcholocationDurationBase.Value = (float)MappingEcholocationDurationBase.DefaultValue;
        MappingEcholocationDurationFirst.Value = (float)MappingEcholocationDurationFirst.DefaultValue;
        MappingEcholocationDurationSecond.Value = (float)MappingEcholocationDurationSecond.DefaultValue;
        MappingCoinMultiplierBase.Value = (float)MappingCoinMultiplierBase.DefaultValue;
        MappingCoinMultiplierFirst.Value = (float)MappingCoinMultiplierFirst.DefaultValue;
        MappingCoinMultiplierSecond.Value = (float)MappingCoinMultiplierSecond.DefaultValue;
        SnoopingOutlineColorHex.Value = (string)SnoopingOutlineColorHex.DefaultValue;
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