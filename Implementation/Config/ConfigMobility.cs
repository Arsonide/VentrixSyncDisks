using BepInEx.Configuration;

namespace VentVigilante.Implementation.Config;

public static partial class VentrixConfig
{
    public static ConfigEntry<float> RunnerSpeedMultiplierBase;
    public static ConfigEntry<float> RunnerSpeedMultiplierFirst;
    public static ConfigEntry<float> RunnerSpeedMultiplierSecond;

    public static ConfigEntry<float> ParkourInteractRangeBase;
    public static ConfigEntry<float> ParkourInteractRangeFirst;
    public static ConfigEntry<float> ParkourInteractRangeSecond;
    
    public static ConfigEntry<float> ParkourTransitionSpeedBase;
    public static ConfigEntry<float> ParkourTransitionSpeedFirst;
    public static ConfigEntry<float> ParkourTransitionSpeedSecond;
    
    public static ConfigEntry<bool> ParkourAutoCloseBase;
    public static ConfigEntry<bool> ParkourAutoCloseFirst;
    public static ConfigEntry<bool> ParkourAutoCloseSecond;

    private static void InitializeMobility(ConfigFile config)
    {
        RunnerSpeedMultiplierBase = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Base Level)", 3f,
                                                new ConfigDescription($"The multiplier on your movement speed in vents with the base level of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierFirst = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (First Upgrade)", 5f,
                                                 new ConfigDescription($"The multiplier on your movement speed in vents with the first upgrade of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierSecond = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Second Upgrade)", 7f,
                                                  new ConfigDescription($"The multiplier on your movement speed in vents with the second upgrade of {NAME_SHORT_RUNNER}."));
        
        ParkourInteractRangeBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Base Level)", 0f,
                                               new ConfigDescription($"How much further you can reach vents with the base level of {NAME_SHORT_PARKOUR}."));
        
        ParkourInteractRangeFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (First Upgrade)", 1f,
                                                new ConfigDescription($"How much further you can reach vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourInteractRangeSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Second Upgrade)", 1f,
                                                 new ConfigDescription($"How much further you can reach vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourTransitionSpeedBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Base Level)", 0.5f,
                                                 new ConfigDescription($"A multiplier on the speed you enter and exit vents with the base level of {NAME_SHORT_PARKOUR}."));

        ParkourTransitionSpeedFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (First Upgrade)", 0.5f,
                                                  new ConfigDescription($"A multiplier on the speed you enter and exit vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourTransitionSpeedSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Second Upgrade)", 0.5f,
                                                   new ConfigDescription($"A multiplier on the speed you enter and exit vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourAutoCloseBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Base Level)", false,
                                           new ConfigDescription($"Whether the base level of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        ParkourAutoCloseFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (First Upgrade)", false,
                                            new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        ParkourAutoCloseSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Second Upgrade)", true,
                                             new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
    }

    public static void ResetRecon()
    {
        RunnerSpeedMultiplierBase.Value = (float)RunnerSpeedMultiplierBase.DefaultValue;
        RunnerSpeedMultiplierFirst.Value = (float)RunnerSpeedMultiplierFirst.DefaultValue;
        RunnerSpeedMultiplierSecond.Value = (float)RunnerSpeedMultiplierSecond.DefaultValue;
        ParkourInteractRangeBase.Value = (float)ParkourInteractRangeBase.DefaultValue;
        ParkourInteractRangeFirst.Value = (float)ParkourInteractRangeFirst.DefaultValue;
        ParkourInteractRangeSecond.Value = (float)ParkourInteractRangeSecond.DefaultValue;
        ParkourTransitionSpeedBase.Value = (float)ParkourTransitionSpeedBase.DefaultValue;
        ParkourTransitionSpeedFirst.Value = (float)ParkourTransitionSpeedFirst.DefaultValue;
        ParkourTransitionSpeedSecond.Value = (float)ParkourTransitionSpeedSecond.DefaultValue;
        ParkourAutoCloseBase.Value = (bool)ParkourAutoCloseBase.DefaultValue;
        ParkourAutoCloseFirst.Value = (bool)ParkourAutoCloseFirst.DefaultValue;
        ParkourAutoCloseSecond.Value = (bool)ParkourAutoCloseSecond.DefaultValue;
    }
}