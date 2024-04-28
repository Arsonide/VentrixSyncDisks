using BepInEx.Configuration;

namespace VentVigilante.Implementation.Config;

public static partial class VentrixConfig
{
    public static ConfigEntry<int> MappingEcholocationRangeBase;
    public static ConfigEntry<int> MappingEcholocationRangeFirst;
    public static ConfigEntry<int> MappingEcholocationRangeSecond;

    public static ConfigEntry<float> MappingEcholocationSpeedBase;
    public static ConfigEntry<float> MappingEcholocationSpeedFirst;
    public static ConfigEntry<float> MappingEcholocationSpeedSecond;
    
    public static ConfigEntry<float> MappingEcholocationDurationBase;
    public static ConfigEntry<float> MappingEcholocationDurationFirst;
    public static ConfigEntry<float> MappingEcholocationDurationSecond;
    
    public static ConfigEntry<float> MappingCoinMultiplierBase;
    public static ConfigEntry<float> MappingCoinMultiplierFirst;
    public static ConfigEntry<float> MappingCoinMultiplierSecond;

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
    }

    public static void ResetMobility()
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
}