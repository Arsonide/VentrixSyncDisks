using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SPECTER = "Shaft Specter";
    private const string NAME_SHORT_SPECTER = "Specter";
    private const int ID_SPECTER = 6;

    public static ConfigCacheDiskEffect<float> SpecterFootstepChance;
    public static ConfigCacheDiskEffect<bool> SpecterColdImmunity;

    private static ConfigEntry<float> _specterFootstepChanceBase;
    private static ConfigEntry<float> _specterFootstepChanceFirst;
    private static ConfigEntry<float> _specterFootstepChanceSecond;

    private static ConfigEntry<bool> _specterColdImmunityBase;
    private static ConfigEntry<bool> _specterColdImmunityFirst;
    private static ConfigEntry<bool> _specterColdImmunitySecond;

    private static void InitializeSpecter(ConfigFile config)
    {
        string section = $"{ID_SPECTER}. {NAME_SHORT_SPECTER}";
        
        const string FOOTSTEP_CHANCE_TITLE = "Footstep Chance";
        const string FOOTSTEP_CHANCE_DESCRIPTION = "The chance you play footstep sounds when travelling in vents, when you normally would,";
        
        _specterFootstepChanceBase = config.Bind(section, $"{FOOTSTEP_CHANCE_TITLE} {LEVEL_1_TITLE}", 0.68f,
                                                new ConfigDescription($"{FOOTSTEP_CHANCE_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SPECTER}.",
                                                                      new AcceptableValueRange<float>(0f, 1f)));

        _specterFootstepChanceFirst = config.Bind(section, $"{FOOTSTEP_CHANCE_TITLE} {LEVEL_2_TITLE}", 0.36f,
                                                 new ConfigDescription($"{FOOTSTEP_CHANCE_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SPECTER}.",
                                                                       new AcceptableValueRange<float>(0f, 1f)));

        _specterFootstepChanceSecond = config.Bind(section, $"{FOOTSTEP_CHANCE_TITLE} {LEVEL_3_TITLE}", 0.04f,
                                                  new ConfigDescription($"{FOOTSTEP_CHANCE_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SPECTER}.",
                                                                        new AcceptableValueRange<float>(0f, 1f)));

        const string COLD_IMMUNITY_TITLE = "Cold Immunity";
        const string COLD_IMMUNITY_DESCRIPTION = "Whether you are granted cold immunity in vents";
        
        _specterColdImmunityBase = config.Bind(section, $"{COLD_IMMUNITY_TITLE} {LEVEL_1_TITLE}", false,
                                          new ConfigDescription($"{COLD_IMMUNITY_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SPECTER}."));
        
        _specterColdImmunityFirst = config.Bind(section, $"{COLD_IMMUNITY_TITLE} {LEVEL_2_TITLE}", true,
                                                new ConfigDescription($"{COLD_IMMUNITY_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SPECTER}."));
        
        _specterColdImmunitySecond = config.Bind(section, $"{COLD_IMMUNITY_TITLE} {LEVEL_3_TITLE}", true,
                                                 new ConfigDescription($"{COLD_IMMUNITY_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SPECTER}."));
    }
    
    private static void CacheSpecter()
    {
        SpecterFootstepChance = new ConfigCacheDiskEffect<float>(1f,
                                                       (level, oldValue, newValue) => $"You make {Utilities.DirectMultiplierDescription(newValue, "less", "more", out string description)}% {description} noise moving through vents.",
                                                       _specterFootstepChanceBase, _specterFootstepChanceFirst, _specterFootstepChanceSecond);
        
        SpecterColdImmunity = new ConfigCacheDiskEffect<bool>(false,
                                                    (level, oldValue, newValue) => $"You {(newValue ? "no longer" : "now")} get cold when in vents.",
                                                    _specterColdImmunityBase, _specterColdImmunityFirst, _specterColdImmunitySecond);
    }

    private static void ResetSpecter()
    {
        _specterFootstepChanceBase.Value = (float)_specterFootstepChanceBase.DefaultValue;
        _specterFootstepChanceFirst.Value = (float)_specterFootstepChanceFirst.DefaultValue;
        _specterFootstepChanceSecond.Value = (float)_specterFootstepChanceSecond.DefaultValue;
        _specterColdImmunityBase.Value = (bool)_specterColdImmunityBase.DefaultValue;
        _specterColdImmunityFirst.Value = (bool)_specterColdImmunityFirst.DefaultValue;
        _specterColdImmunitySecond.Value = (bool)_specterColdImmunitySecond.DefaultValue;
    }
}