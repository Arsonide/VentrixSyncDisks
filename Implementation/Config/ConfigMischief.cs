using BepInEx.Configuration;

namespace VentVigilante.Implementation.Config;

public static partial class VentrixConfig
{
    public static ConfigEntry<float> SpecterFootstepChanceBase;
    public static ConfigEntry<float> SpecterFootstepChanceFirst;
    public static ConfigEntry<float> SpecterFootstepChanceSecond;
    
    public static ConfigEntry<bool> SpecterColdImmunityBase;
    public static ConfigEntry<bool> SpecterColdImmunityFirst;
    public static ConfigEntry<bool> SpecterColdImmunitySecond;

    public static ConfigEntry<float> MenaceCitizenNerveBase;
    public static ConfigEntry<float> MenaceCitizenNerveFirst;
    public static ConfigEntry<float> MenaceNerveLevelSecond;

    public static ConfigEntry<bool> MenaceToxicImmunityBase;
    public static ConfigEntry<bool> MenaceToxicImmunityFirst;
    public static ConfigEntry<bool> MenaceToxicImmunitySecond;

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

        MenaceNerveLevelSecond = config.Bind($"7. {NAME_SHORT_MENACE}", "Citizen Nerve (Second Upgrade)", 0.1f,
                                             new ConfigDescription($"The amount a citizen's nerve is set to when you pop out of vents with the second upgrade of {NAME_SHORT_MENACE}."));
        
        MenaceToxicImmunityBase = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunityFirst = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunitySecond = config.Bind($"7. {NAME_SHORT_MENACE}", "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
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
        MenaceNerveLevelSecond.Value = (float)MenaceNerveLevelSecond.DefaultValue;
        MenaceToxicImmunityBase.Value = (bool)MenaceToxicImmunityBase.DefaultValue;
        MenaceToxicImmunityFirst.Value = (bool)MenaceToxicImmunityFirst.DefaultValue;
        MenaceToxicImmunitySecond.Value = (bool)MenaceToxicImmunitySecond.DefaultValue;
    }
}