using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SNOOPING = "Grate Snooping";
    private const string NAME_SHORT_SNOOPING = "Snooping";
    private const int ID_SNOOPING = 6;

    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansBase;
    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansSecond;
    public static ConfigCache<bool> SnoopingCanSnoopCivilians;
    
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksBase;
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksSecond;
    public static ConfigCache<bool> SnoopingCanSnoopPeeks;
    
    private static ConfigEntry<bool> SnoopingCanSnoopSecurityBase;
    private static ConfigEntry<bool> SnoopingCanSnoopSecurityFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopSecuritySecond;
    public static ConfigCache<bool> SnoopingCanSnoopSecurity;
    
    private static ConfigEntry<bool> SnoopingCanPassTimeBase;
    private static ConfigEntry<bool> SnoopingCanPassTimeFirst;
    private static ConfigEntry<bool> SnoopingCanPassTimeSecond;
    public static ConfigCache<bool> SnoopingCanPassTime;

    public static ConfigEntry<float> SnoopingPassTimeWarpDelay;
    public static ConfigEntry<float> SnoopingPassTimeNotificationDelay;

    private static void InitializeSnooping(ConfigFile config)
    {
        SnoopingCanSnoopCiviliansBase = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (Base Level)", true,
                                                    new ConfigDescription($"Whether you see civilians through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopCiviliansFirst = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (First Upgrade)", true,
                                                     new ConfigDescription($"Whether you see civilians through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopCiviliansSecond = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (Second Upgrade)", true,
                                                      new ConfigDescription($"Whether you see civilians through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));

        SnoopingCanSnoopPeeksBase = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (Base Level)", false,
                                                    new ConfigDescription($"Whether you see things through walls when near \"peek\" vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopPeeksFirst = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (First Upgrade)", false,
                                                     new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopPeeksSecond = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (Second Upgrade)", false,
                                                      new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecurityBase = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (Base Level)", false,
                                                new ConfigDescription($"Whether you see security systems through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecurityFirst = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (First Upgrade)", true,
                                                    new ConfigDescription($"Whether you see security systems through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecuritySecond = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (Second Upgrade)", true,
                                                     new ConfigDescription($"Whether you see security systems through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanPassTimeBase = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Pass Time Near Vents (Base Level)", false,
                                                   new ConfigDescription($"Whether you can stare at your watch to pass time when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanPassTimeFirst = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Pass Time Near Vents (First Upgrade)", false,
                                              new ConfigDescription($"Whether you can stare at your watch to pass time when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanPassTimeSecond = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Can Pass Time Near Vents (Second Upgrade)", true,
                                              new ConfigDescription($"Whether you can stare at your watch to pass time when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingPassTimeWarpDelay = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Pass Time Warp Delay", 10f,
                                             new ConfigDescription($"When using {NAME_LONG_SNOOPING}, how long you must stare at your watch to pass time near vents."));
        
        SnoopingPassTimeNotificationDelay = config.Bind($"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}", "Pass Time Notification Delay", 5f,
                                             new ConfigDescription($"When using {NAME_LONG_SNOOPING}, when to notify you while staring at your watch that you are about to pass time near vents. (Set to negative number for no notification.)"));
        
        // Setup Caches
        SnoopingCanSnoopCivilians = new ConfigCache<bool>(false,
                                                          (level, oldValue, newValue) =>
                                                              $"You can now see unaware civilians through walls when near vent entrances.",
                                                          SnoopingCanSnoopCiviliansBase, SnoopingCanSnoopCiviliansFirst, SnoopingCanSnoopCiviliansSecond);
        
        SnoopingCanSnoopPeeks = new ConfigCache<bool>(false,
                                                          (level, oldValue, newValue) =>
                                                              $"Snooping now also applies when near \"peeking\" vents in the middle of air ducts.",
                                                          SnoopingCanSnoopPeeksBase, SnoopingCanSnoopPeeksFirst, SnoopingCanSnoopPeeksSecond);
        
        SnoopingCanSnoopSecurity = new ConfigCache<bool>(false,
                                                      (level, oldValue, newValue) =>
                                                          $"You can now see cameras, laser sensors, sentry guns, and gas dispensers through walls when near vent entrances.",
                                                      SnoopingCanSnoopSecurityBase, SnoopingCanSnoopSecurityFirst, SnoopingCanSnoopSecuritySecond);
        
        SnoopingCanPassTime = new ConfigCache<bool>(false,
                                                         (level, oldValue, newValue) =>
                                                             $"When in air ducts near vents, you can now stare at your watch to pass time faster.",
                                                         SnoopingCanPassTimeBase, SnoopingCanPassTimeFirst, SnoopingCanPassTimeSecond);
    }

    public static void ResetSnooping()
    {
        SnoopingCanSnoopCiviliansBase.Value = (bool)SnoopingCanSnoopCiviliansBase.DefaultValue;
        SnoopingCanSnoopCiviliansFirst.Value = (bool)SnoopingCanSnoopCiviliansFirst.DefaultValue;
        SnoopingCanSnoopCiviliansSecond.Value = (bool)SnoopingCanSnoopCiviliansSecond.DefaultValue;
        SnoopingCanSnoopPeeksBase.Value = (bool)SnoopingCanSnoopPeeksBase.DefaultValue;
        SnoopingCanSnoopPeeksFirst.Value = (bool)SnoopingCanSnoopPeeksFirst.DefaultValue;
        SnoopingCanSnoopPeeksSecond.Value = (bool)SnoopingCanSnoopPeeksSecond.DefaultValue;
        SnoopingCanSnoopSecurityBase.Value = (bool)SnoopingCanSnoopSecurityBase.DefaultValue;
        SnoopingCanSnoopSecurityFirst.Value = (bool)SnoopingCanSnoopSecurityFirst.DefaultValue;
        SnoopingCanSnoopSecuritySecond.Value = (bool)SnoopingCanSnoopSecuritySecond.DefaultValue;
        SnoopingCanPassTimeBase.Value = (bool)SnoopingCanPassTimeBase.DefaultValue;
        SnoopingCanPassTimeFirst.Value = (bool)SnoopingCanPassTimeFirst.DefaultValue;
        SnoopingCanPassTimeSecond.Value = (bool)SnoopingCanPassTimeSecond.DefaultValue;
        SnoopingPassTimeWarpDelay.Value = (float)SnoopingPassTimeWarpDelay.DefaultValue;
        SnoopingPassTimeNotificationDelay.Value = (float)SnoopingPassTimeNotificationDelay.DefaultValue;
    }
}