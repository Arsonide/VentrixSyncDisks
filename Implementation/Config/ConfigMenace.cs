using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_MENACE = "Shaft Menace";
    private const string NAME_SHORT_MENACE = "Menace";
    private const int ID_MENACE = 8;
    
    private static ConfigEntry<int> MenaceFreakoutDurationBase;
    private static ConfigEntry<int> MenaceFreakoutDurationFirst;
    private static ConfigEntry<int> MenaceFreakoutDurationSecond;
    public static ConfigCache<int> MenaceFreakoutDuration;

    private static ConfigEntry<bool> MenaceToxicImmunityBase;
    private static ConfigEntry<bool> MenaceToxicImmunityFirst;
    private static ConfigEntry<bool> MenaceToxicImmunitySecond;
    public static ConfigCache<bool> MenaceToxicImmunity;

    private static void InitializeMenace(ConfigFile config)
    {
        MenaceFreakoutDurationBase = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Freakout Duration (Base Level)", 4,
                                                 new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the base level of {NAME_SHORT_MENACE}."));

        MenaceFreakoutDurationFirst = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Freakout Duration (First Upgrade)", 8,
                                            new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the first upgrade of {NAME_SHORT_MENACE}."));

        MenaceFreakoutDurationSecond = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Freakout Duration (Second Upgrade)", 12,
                                             new ConfigDescription($"The duration a citizen freaks out when you pop out of vents in private areas with the second upgrade of {NAME_SHORT_MENACE}."));
        
        MenaceToxicImmunityBase = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Toxic Immunity (Base Level)", false,
                                          new ConfigDescription($"Whether the base level of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunityFirst = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Toxic Immunity (First Upgrade)", true,
                                              new ConfigDescription($"Whether the first upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        MenaceToxicImmunitySecond = config.Bind($"{ID_MENACE}. {NAME_SHORT_MENACE}", "Toxic Immunity (Second Upgrade)", true,
                                              new ConfigDescription($"Whether the second upgrade of {NAME_SHORT_MENACE} grants you toxic gas immunity in vents."));
        
        // Setup Caches
        MenaceFreakoutDuration = new ConfigCache<int>(1,
                                                      (level, oldValue, newValue) => $"Popping out of vents in private rooms makes citizens awake in that room freak out for {newValue} seconds.",
                                                      MenaceFreakoutDurationBase, MenaceFreakoutDurationFirst, MenaceFreakoutDurationSecond);
        
        MenaceToxicImmunity = new ConfigCache<bool>(false,
                                                    (level, oldValue, newValue) => $"You are {(newValue ? "no longer" : "now")} affected by toxic gas in vents.",
                                                    MenaceToxicImmunityBase, MenaceToxicImmunityFirst, MenaceToxicImmunitySecond);
    }

    public static void ResetMenace()
    {
        MenaceFreakoutDurationBase.Value = (int)MenaceFreakoutDurationBase.DefaultValue;
        MenaceFreakoutDurationFirst.Value = (int)MenaceFreakoutDurationFirst.DefaultValue;
        MenaceFreakoutDurationSecond.Value = (int)MenaceFreakoutDurationSecond.DefaultValue;
        MenaceToxicImmunityBase.Value = (bool)MenaceToxicImmunityBase.DefaultValue;
        MenaceToxicImmunityFirst.Value = (bool)MenaceToxicImmunityFirst.DefaultValue;
        MenaceToxicImmunitySecond.Value = (bool)MenaceToxicImmunitySecond.DefaultValue;
    }
}