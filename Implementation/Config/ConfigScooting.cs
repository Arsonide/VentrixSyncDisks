using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SCOOTING = "Chute Scooting";
    private const string NAME_SHORT_SCOOTING = "Scooting";
    private const int ID_SCOOTING = 2;
    
    public static ConfigCacheDiskEffect<float> ScootingSpeedMultiplier;

    private static ConfigEntry<float> _scootingSpeedMultiplierBase;
    private static ConfigEntry<float> _scootingSpeedMultiplierFirst;
    private static ConfigEntry<float> _scootingSpeedMultiplierSecond;

    private static void InitializeScooting(ConfigFile config)
    {
        string section = $"{ID_SCOOTING}. {NAME_SHORT_SCOOTING}";

        _scootingSpeedMultiplierBase = config.Bind(section, "Vent Speed Multiplier (Base Level)", 1.5f,
                                                   new ConfigDescription($"The multiplier on your movement speed in vents with the base level of {NAME_SHORT_SCOOTING}."));
        
        _scootingSpeedMultiplierFirst = config.Bind(section, "Vent Speed Multiplier (First Upgrade)", 2f,
                                                    new ConfigDescription($"The multiplier on your movement speed in vents with the first upgrade of {NAME_SHORT_SCOOTING}."));
        
        _scootingSpeedMultiplierSecond = config.Bind(section, "Vent Speed Multiplier (Second Upgrade)", 2.5f,
                                                     new ConfigDescription($"The multiplier on your movement speed in vents with the second upgrade of {NAME_SHORT_SCOOTING}."));
    }

    private static void CacheScooting()
    {
        ScootingSpeedMultiplier = new ConfigCacheDiskEffect<float>(1f,
                                                         (level, oldValue, newValue) => $"You now move {Utilities.MultiplierForDescription(newValue, "slower", "faster", out string description)}% {description} through vents.",
                                                         _scootingSpeedMultiplierBase, _scootingSpeedMultiplierFirst, _scootingSpeedMultiplierSecond);
    }

    private static void ResetScooting()
    {
        _scootingSpeedMultiplierBase.Value = (float)_scootingSpeedMultiplierBase.DefaultValue;
        _scootingSpeedMultiplierFirst.Value = (float)_scootingSpeedMultiplierFirst.DefaultValue;
        _scootingSpeedMultiplierSecond.Value = (float)_scootingSpeedMultiplierSecond.DefaultValue;
    }
}