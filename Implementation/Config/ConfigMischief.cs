using BepInEx.Configuration;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private static ConfigEntry<float> SpecterFootstepChanceBase;
    private static ConfigEntry<float> SpecterFootstepChanceFirst;
    private static ConfigEntry<float> SpecterFootstepChanceSecond;
    public static ConfigCache<float> SpecterFootstepChance;

    private static ConfigEntry<bool> SpecterColdImmunityBase;
    private static ConfigEntry<bool> SpecterColdImmunityFirst;
    private static ConfigEntry<bool> SpecterColdImmunitySecond;
    public static ConfigCache<bool> SpecterColdImmunity;

    private static ConfigEntry<float> MenaceCitizenNerveBase;
    private static ConfigEntry<float> MenaceCitizenNerveFirst;
    private static ConfigEntry<float> MenaceCitizenNerveSecond;
    public static ConfigCache<float> MenaceCitizenNerve;

    private static ConfigEntry<bool> MenaceToxicImmunityBase;
    private static ConfigEntry<bool> MenaceToxicImmunityFirst;
    private static ConfigEntry<bool> MenaceToxicImmunitySecond;
    public static ConfigCache<bool> MenaceToxicImmunity;

    private static void InitializeMischief(ConfigFile config)
    {
        SpecterFootstepChanceBase = config.Bind($"6. {NAME_SHORT_SPECTER}", "Footstep Chance (Base Level)", 0.68f,
                                                new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the base level of {NAME_SHORT_SPECTER}.",
                                                                      new AcceptableValueRange<float>(0f, 1f)));

        SpecterFootstepChanceFirst = config.Bind($"6. {NAME_SHORT_SPECTER}", "Footstep Chance (First Upgrade)", 0.36f,
                                                 new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the first upgrade of {NAME_SHORT_SPECTER}.",
                                                                       new AcceptableValueRange<float>(0f, 1f)));

        SpecterFootstepChanceSecond = config.Bind($"6. {NAME_SHORT_SPECTER}", "Footstep Chance (Second Upgrade)", 0.04f,
                                                  new ConfigDescription($"The chance you play footstep sounds when travelling in vents, when you normally would, with the second upgrade of {NAME_SHORT_SPECTER}.",
                                                                        new AcceptableValueRange<float>(0f, 1f)));

        SpecterColdImmunityBase = config.Bind($"6. {NAME_SHORT_SPECTER}", "Cold Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        SpecterColdImmunityFirst = config.Bind($"6. {NAME_SHORT_SPECTER}", "Cold Immunity (First Upgrade)", true,
                                          new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));
        
        SpecterColdImmunitySecond = config.Bind($"6. {NAME_SHORT_SPECTER}", "Cold Immunity (Second Upgrade)", true,
                                          new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_SPECTER} grants you cold immunity in vents."));

        MenaceCitizenNerveBase = config.Bind($"7. {NAME_SHORT_MENACE}", "Citizen Nerve (Base Level)", 0.3f,
                                             new ConfigDescription($"The amount a citizen's nerve is set to when you pop out of vents with the base level of {NAME_SHORT_MENACE}."));

        MenaceCitizenNerveFirst = config.Bind($"7. {NAME_SHORT_MENACE}", "Citizen Nerve (First Upgrade)", 0.2f,
                                            new ConfigDescription($"The amount a citizen's nerve is set to when you pop out of vents with the first upgrade of {NAME_SHORT_MENACE}."));

        MenaceCitizenNerveSecond = config.Bind($"7. {NAME_SHORT_MENACE}", "Citizen Nerve (Second Upgrade)", 0.1f,
                                             new ConfigDescription($"The amount a citizen's nerve is set to when you pop out of vents with the second upgrade of {NAME_SHORT_MENACE}."));
        
        MenaceToxicImmunityBase = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunityFirst = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunitySecond = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        // Setup Caches
        SpecterFootstepChance = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) => $"You make {Utilities.MultiplierForDescription(newValue, "less", "more", out string description)}% {description} noise moving through vents.",
                                                       SpecterFootstepChanceBase, SpecterFootstepChanceFirst, SpecterFootstepChanceSecond);
        
        SpecterColdImmunity = new ConfigCache<bool>(false,
                                                     (level, oldValue, newValue) => $"You {(newValue ? "no longer" : "now")} get cold when in vents.",
                                                     SpecterColdImmunityBase, SpecterColdImmunityFirst, SpecterColdImmunitySecond);
        
        MenaceCitizenNerve = new ConfigCache<float>(1f,
                                                  (level, oldValue, newValue) => $"Popping out of vents in private rooms lowers nerves of people by {Mathf.RoundToInt(newValue)}%.",
                                                  MenaceCitizenNerveBase, MenaceCitizenNerveFirst, MenaceCitizenNerveSecond);
        
        MenaceToxicImmunity = new ConfigCache<bool>(false,
                                                    (level, oldValue, newValue) => $"You are {(newValue ? "no longer" : "now")} affected by toxic gas in vents.",
                                                    MenaceToxicImmunityBase, MenaceToxicImmunityFirst, MenaceToxicImmunitySecond);
    }

    public static void ResetMischief()
    {
        SpecterFootstepChanceBase.Value = (float)SpecterFootstepChanceBase.DefaultValue;
        SpecterFootstepChanceFirst.Value = (float)SpecterFootstepChanceFirst.DefaultValue;
        SpecterFootstepChanceSecond.Value = (float)SpecterFootstepChanceSecond.DefaultValue;
        SpecterColdImmunityBase.Value = (bool)SpecterColdImmunityBase.DefaultValue;
        SpecterColdImmunityFirst.Value = (bool)SpecterColdImmunityFirst.DefaultValue;
        SpecterColdImmunitySecond.Value = (bool)SpecterColdImmunitySecond.DefaultValue;
        MenaceCitizenNerveBase.Value = (float)MenaceCitizenNerveBase.DefaultValue;
        MenaceCitizenNerveFirst.Value = (float)MenaceCitizenNerveFirst.DefaultValue;
        MenaceCitizenNerveSecond.Value = (float)MenaceCitizenNerveSecond.DefaultValue;
        MenaceToxicImmunityBase.Value = (bool)MenaceToxicImmunityBase.DefaultValue;
        MenaceToxicImmunityFirst.Value = (bool)MenaceToxicImmunityFirst.DefaultValue;
        MenaceToxicImmunitySecond.Value = (bool)MenaceToxicImmunitySecond.DefaultValue;
    }
}