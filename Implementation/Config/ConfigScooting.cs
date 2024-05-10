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

        const string VENT_SPEED_MULTIPLIER_TITLE = "Vent Speed Multiplier";
        const string VENT_SPEED_MULTIPLIER_DESCRIPTION = "The multiplier on your movement speed in vents";
        
        _scootingSpeedMultiplierBase = config.Bind(section, $"{VENT_SPEED_MULTIPLIER_TITLE} {LEVEL_1_TITLE}", 1.5f,
                                                   new ConfigDescription($"{VENT_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SCOOTING}."));
        
        _scootingSpeedMultiplierFirst = config.Bind(section, $"{VENT_SPEED_MULTIPLIER_TITLE} {LEVEL_2_TITLE}", 2f,
                                                    new ConfigDescription($"{VENT_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SCOOTING}."));
        
        _scootingSpeedMultiplierSecond = config.Bind(section, $"{VENT_SPEED_MULTIPLIER_TITLE} {LEVEL_3_TITLE}", 2.5f,
                                                     new ConfigDescription($"{VENT_SPEED_MULTIPLIER_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SCOOTING}."));
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