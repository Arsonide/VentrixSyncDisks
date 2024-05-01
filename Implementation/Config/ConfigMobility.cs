using BepInEx.Configuration;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private static ConfigEntry<float> RunnerSpeedMultiplierBase;
    private static ConfigEntry<float> RunnerSpeedMultiplierFirst;
    private static ConfigEntry<float> RunnerSpeedMultiplierSecond;
    public static ConfigCache<float> RunnerSpeedMultiplier;
    
    private static ConfigEntry<float> ParkourInteractRangeBase;
    private static ConfigEntry<float> ParkourInteractRangeFirst;
    private static ConfigEntry<float> ParkourInteractRangeSecond;
    public static ConfigCache<float> ParkourInteractRange;

    private static ConfigEntry<float> ParkourTransitionSpeedBase;
    private static ConfigEntry<float> ParkourTransitionSpeedFirst;
    private static ConfigEntry<float> ParkourTransitionSpeedSecond;
    public static ConfigCache<float> ParkourTransitionSpeed;

    private static ConfigEntry<bool> ParkourAutoCloseBase;
    private static ConfigEntry<bool> ParkourAutoCloseFirst;
    private static ConfigEntry<bool> ParkourAutoCloseSecond;
    public static ConfigCache<bool> ParkourAutoClose;

    private static void InitializeMobility(ConfigFile config)
    {
        RunnerSpeedMultiplierBase = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Base Level)", 2f,
                                                new ConfigDescription($"The multiplier on your movement speed in vents with the base level of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierFirst = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (First Upgrade)", 3f,
                                                 new ConfigDescription($"The multiplier on your movement speed in vents with the first upgrade of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierSecond = config.Bind($"2. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Second Upgrade)", 4f,
                                                  new ConfigDescription($"The multiplier on your movement speed in vents with the second upgrade of {NAME_SHORT_RUNNER}."));
        
        ParkourInteractRangeBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Base Level)", 0f,
                                               new ConfigDescription($"How much further you can reach vents with the base level of {NAME_SHORT_PARKOUR}."));
        
        ParkourInteractRangeFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (First Upgrade)", 1f,
                                                new ConfigDescription($"How much further you can reach vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourInteractRangeSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Added Interaction Range (Second Upgrade)", 1f,
                                                 new ConfigDescription($"How much further you can reach vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourTransitionSpeedBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Base Level)", 0.75f,
                                                 new ConfigDescription($"A multiplier on the speed you enter and exit vents with the base level of {NAME_SHORT_PARKOUR}."));

        ParkourTransitionSpeedFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (First Upgrade)", 0.75f,
                                                  new ConfigDescription($"A multiplier on the speed you enter and exit vents with the first upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourTransitionSpeedSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Transition Speed Multiplier (Second Upgrade)", 0.75f,
                                                   new ConfigDescription($"A multiplier on the speed you enter and exit vents with the second upgrade of {NAME_SHORT_PARKOUR}."));
        
        ParkourAutoCloseBase = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Base Level)", false,
                                           new ConfigDescription($"Whether the base level of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        ParkourAutoCloseFirst = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (First Upgrade)", false,
                                            new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));
        
        ParkourAutoCloseSecond = config.Bind($"3. {NAME_SHORT_PARKOUR}", "Auto Close Vents (Second Upgrade)", true,
                                             new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_PARKOUR} auto closes vents you enter and exit."));

        // Setup Caches
        RunnerSpeedMultiplier = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) => $"You now move {Mathf.RoundToInt(newValue)}x {(newValue > 1 ? "faster" : "slower")} through vents.",
                                                       RunnerSpeedMultiplierBase, RunnerSpeedMultiplierFirst, RunnerSpeedMultiplierSecond);
        
        ParkourInteractRange = new ConfigCache<float>(0f,
                                                       (level, oldValue, newValue) => $"You can now reach vents {newValue}m further away.",
                                                       ParkourInteractRangeBase, ParkourInteractRangeFirst, ParkourInteractRangeSecond);
        
        ParkourTransitionSpeed = new ConfigCache<float>(1f,
                                                        (level, oldValue, newValue) => $"You get in and out of vents {Utilities.MultiplierForDescription(newValue, "faster", "slower", out string description)}% {description}.",
                                                        ParkourTransitionSpeedBase, ParkourTransitionSpeedFirst, ParkourTransitionSpeedSecond);
        
        ParkourAutoClose = new ConfigCache<bool>(false,
                                                        (level, oldValue, newValue) => $"Vents {(newValue ? "now" : "no longer")} automatically close when entering or exiting them.",
                                                        ParkourAutoCloseBase, ParkourAutoCloseFirst, ParkourAutoCloseSecond);
    }

    public static void ResetMobility()
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