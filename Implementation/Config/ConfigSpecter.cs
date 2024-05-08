using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SPECTER = "Crawlspace Specter";
    private const string NAME_SHORT_SPECTER = "Specter";
    private const int ID_SPECTER = 7;

    private static ConfigEntry<float> SpecterFootstepChanceBase;
    private static ConfigEntry<float> SpecterFootstepChanceFirst;
    private static ConfigEntry<float> SpecterFootstepChanceSecond;
    public static ConfigCache<float> SpecterFootstepChance;

    private static ConfigEntry<bool> SpecterColdImmunityBase;
    private static ConfigEntry<bool> SpecterColdImmunityFirst;
    private static ConfigEntry<bool> SpecterColdImmunitySecond;
    public static ConfigCache<bool> SpecterColdImmunity;

    private static void InitializeSpecter(ConfigFile config)
    {
        SpecterFootstepChanceBase = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Footstep Chance (Base Level)", 0.68f,
                                                new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the base level of {NAME_SHORT_SPECTER}.",
                                                                      new AcceptableValueRange<float>(0f, 1f)));

        SpecterFootstepChanceFirst = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Footstep Chance (First Upgrade)", 0.36f,
                                                 new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the first upgrade of {NAME_SHORT_SPECTER}.",
                                                                       new AcceptableValueRange<float>(0f, 1f)));

        SpecterFootstepChanceSecond = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Footstep Chance (Second Upgrade)", 0.04f,
                                                  new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the second upgrade of {NAME_SHORT_SPECTER}.",
                                                                        new AcceptableValueRange<float>(0f, 1f)));

        SpecterColdImmunityBase = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Cold Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        SpecterColdImmunityFirst = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Cold Immunity (First Upgrade)", true,
                                          new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        SpecterColdImmunitySecond = config.Bind($"{ID_SPECTER}. {NAME_SHORT_SPECTER}", "Cold Immunity (Second Upgrade)", true,
                                          new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));

        // Setup Caches
        SpecterFootstepChance = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) => $"You make {Utilities.MultiplierForDescription(newValue, "less", "more", out string description)}% {description} noise moving through vents.",
                                                       SpecterFootstepChanceBase, SpecterFootstepChanceFirst, SpecterFootstepChanceSecond);
        
        SpecterColdImmunity = new ConfigCache<bool>(false,
                                                     (level, oldValue, newValue) => $"You {(newValue ? "no longer" : "now")} get cold when in vents.",
                                                     SpecterColdImmunityBase, SpecterColdImmunityFirst, SpecterColdImmunitySecond);
    }

    public static void ResetSpecter()
    {
        SpecterFootstepChanceBase.Value = (float)SpecterFootstepChanceBase.DefaultValue;
        SpecterFootstepChanceFirst.Value = (float)SpecterFootstepChanceFirst.DefaultValue;
        SpecterFootstepChanceSecond.Value = (float)SpecterFootstepChanceSecond.DefaultValue;
        SpecterColdImmunityBase.Value = (bool)SpecterColdImmunityBase.DefaultValue;
        SpecterColdImmunityFirst.Value = (bool)SpecterColdImmunityFirst.DefaultValue;
        SpecterColdImmunitySecond.Value = (bool)SpecterColdImmunitySecond.DefaultValue;
    }
}