using BepInEx.Configuration;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_RUNNER = "Airway Runner";
    private const string NAME_SHORT_RUNNER = "Runner";
    private const int ID_RUNNER = 2;

    private static ConfigEntry<float> RunnerSpeedMultiplierBase;
    private static ConfigEntry<float> RunnerSpeedMultiplierFirst;
    private static ConfigEntry<float> RunnerSpeedMultiplierSecond;
    public static ConfigCache<float> RunnerSpeedMultiplier;

    private static void InitializeRunner(ConfigFile config)
    {
        RunnerSpeedMultiplierBase = config.Bind($"{ID_RUNNER}. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Base Level)", 1.5f,
                                                new ConfigDescription($"The multiplier on your movement speed in vents with the base level of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierFirst = config.Bind($"{ID_RUNNER}. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (First Upgrade)", 2f,
                                                 new ConfigDescription($"The multiplier on your movement speed in vents with the first upgrade of {NAME_SHORT_RUNNER}."));
        
        RunnerSpeedMultiplierSecond = config.Bind($"{ID_RUNNER}. {NAME_SHORT_RUNNER}", "Vent Speed Multiplier (Second Upgrade)", 2.5f,
                                                  new ConfigDescription($"The multiplier on your movement speed in vents with the second upgrade of {NAME_SHORT_RUNNER}."));

        // Setup Caches
        RunnerSpeedMultiplier = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) => $"You now move {Utilities.MultiplierForDescription(newValue, "slower", "faster", out string description)}% {description} through vents.",
                                                       RunnerSpeedMultiplierBase, RunnerSpeedMultiplierFirst, RunnerSpeedMultiplierSecond);
    }

    public static void ResetRunner()
    {
        RunnerSpeedMultiplierBase.Value = (float)RunnerSpeedMultiplierBase.DefaultValue;
        RunnerSpeedMultiplierFirst.Value = (float)RunnerSpeedMultiplierFirst.DefaultValue;
        RunnerSpeedMultiplierSecond.Value = (float)RunnerSpeedMultiplierSecond.DefaultValue;
    }
}