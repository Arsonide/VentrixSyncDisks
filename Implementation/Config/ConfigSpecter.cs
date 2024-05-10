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

        _specterFootstepChanceBase = config.Bind(section, "Footstep Chance (Base Level)", 0.68f,
                                                new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the base level of {NAME_SHORT_SPECTER}.",
                                                                      new AcceptableValueRange<float>(0f, 1f)));

        _specterFootstepChanceFirst = config.Bind(section, "Footstep Chance (First Upgrade)", 0.36f,
                                                 new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the first upgrade of {NAME_SHORT_SPECTER}.",
                                                                       new AcceptableValueRange<float>(0f, 1f)));

        _specterFootstepChanceSecond = config.Bind(section, "Footstep Chance (Second Upgrade)", 0.04f,
                                                  new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the second upgrade of {NAME_SHORT_SPECTER}.",
                                                                        new AcceptableValueRange<float>(0f, 1f)));

        _specterColdImmunityBase = config.Bind(section, "Cold Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        _specterColdImmunityFirst = config.Bind(section, "Cold Immunity (First Upgrade)", true,
                                          new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        _specterColdImmunitySecond = config.Bind(section, "Cold Immunity (Second Upgrade)", true,
                                          new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
    }
    
    private static void CacheSpecter()
    {
        SpecterFootstepChance = new ConfigCacheDiskEffect<float>(1f,
                                                       (level, oldValue, newValue) => $"You make {Utilities.MultiplierForDescription(newValue, "less", "more", out string description)}% {description} noise moving through vents.",
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