using BepInEx.Configuration;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SCOOTING = "Chute Scooting";
    private const string NAME_SHORT_SCOOTING = "Scooting";
    private const int ID_SCOOTING = 2;

    private static ConfigEntry<float> ScootingSpeedMultiplierBase;
    private static ConfigEntry<float> ScootingSpeedMultiplierFirst;
    private static ConfigEntry<float> ScootingSpeedMultiplierSecond;
    public static ConfigCache<float> ScootingSpeedMultiplier;

    private static void InitializeScooting(ConfigFile config)
    {
        ScootingSpeedMultiplierBase = config.Bind($"{ID_SCOOTING}. {NAME_SHORT_SCOOTING}", "Vent Speed Multiplier (Base Level)", 1.5f,
                                                new ConfigDescription($"The multiplier on your movement speed in vents with the base level of {NAME_SHORT_SCOOTING}."));
        
        ScootingSpeedMultiplierFirst = config.Bind($"{ID_SCOOTING}. {NAME_SHORT_SCOOTING}", "Vent Speed Multiplier (First Upgrade)", 2f,
                                                 new ConfigDescription($"The multiplier on your movement speed in vents with the first upgrade of {NAME_SHORT_SCOOTING}."));
        
        ScootingSpeedMultiplierSecond = config.Bind($"{ID_SCOOTING}. {NAME_SHORT_SCOOTING}", "Vent Speed Multiplier (Second Upgrade)", 2.5f,
                                                  new ConfigDescription($"The multiplier on your movement speed in vents with the second upgrade of {NAME_SHORT_SCOOTING}."));

        // Setup Caches
        ScootingSpeedMultiplier = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) => $"You now move {Utilities.MultiplierForDescription(newValue, "slower", "faster", out string description)}% {description} through vents.",
                                                       ScootingSpeedMultiplierBase, ScootingSpeedMultiplierFirst, ScootingSpeedMultiplierSecond);
    }

    public static void ResetScooting()
    {
        ScootingSpeedMultiplierBase.Value = (float)ScootingSpeedMultiplierBase.DefaultValue;
        ScootingSpeedMultiplierFirst.Value = (float)ScootingSpeedMultiplierFirst.DefaultValue;
        ScootingSpeedMultiplierSecond.Value = (float)ScootingSpeedMultiplierSecond.DefaultValue;
    }
}