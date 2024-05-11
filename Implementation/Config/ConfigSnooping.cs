using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Config.Caches;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_LONG_SNOOPING = "Grate Snooping";
    private const string NAME_SHORT_SNOOPING = "Snooping";
    private const int ID_SNOOPING = 5;
    
    public static ConfigCacheSimple<float> SnoopingPassTimeWarpDelay;
    public static ConfigCacheSimple<float> SnoopingPassTimeNotificationDelay;
    
    public static ConfigCacheDiskEffect<bool> SnoopingCanSnoopCivilians;
    public static ConfigCacheDiskEffect<bool> SnoopingCanSnoopPeeks;
    public static ConfigCacheDiskEffect<bool> SnoopingCanSnoopSecurity;
    public static ConfigCacheDiskEffect<bool> SnoopingCanPassTime;

    private static ConfigEntry<bool> _snoopingCanSnoopCiviliansBase;
    private static ConfigEntry<bool> _snoopingCanSnoopCiviliansFirst;
    private static ConfigEntry<bool> _snoopingCanSnoopCiviliansSecond;
    
    private static ConfigEntry<bool> _snoopingCanSnoopPeeksBase;
    private static ConfigEntry<bool> _snoopingCanSnoopPeeksFirst;
    private static ConfigEntry<bool> _snoopingCanSnoopPeeksSecond;
    
    private static ConfigEntry<bool> _snoopingCanSnoopSecurityBase;
    private static ConfigEntry<bool> _snoopingCanSnoopSecurityFirst;
    private static ConfigEntry<bool> _snoopingCanSnoopSecuritySecond;
    
    private static ConfigEntry<bool> _snoopingCanPassTimeBase;
    private static ConfigEntry<bool> _snoopingCanPassTimeFirst;
    private static ConfigEntry<bool> _snoopingCanPassTimeSecond;

    private static ConfigEntry<float> _snoopingPassTimeWarpDelay;
    private static ConfigEntry<float> _snoopingPassTimeNotificationDelay;
    
    private static void InitializeSnooping(ConfigFile config)
    {
        string section = $"{ID_SNOOPING}. {NAME_SHORT_SNOOPING}";

        const string CAN_SNOOP_CIVILIANS_TITLE = "Can Snoop Civilians";
        const string CAN_SNOOP_CIVILIANS_DESCRIPTION = "Whether you see civilians through walls when near vents";
        
        _snoopingCanSnoopCiviliansBase = config.Bind(section, $"{CAN_SNOOP_CIVILIANS_TITLE} {LEVEL_1_TITLE}", true,
                                                    new ConfigDescription($"{CAN_SNOOP_CIVILIANS_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopCiviliansFirst = config.Bind(section, $"{CAN_SNOOP_CIVILIANS_TITLE} {LEVEL_2_TITLE}", true,
                                                     new ConfigDescription($"{CAN_SNOOP_CIVILIANS_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopCiviliansSecond = config.Bind(section, $"{CAN_SNOOP_CIVILIANS_TITLE} {LEVEL_3_TITLE}", true,
                                                      new ConfigDescription($"{CAN_SNOOP_CIVILIANS_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        const string CAN_SNOOP_PEEKS_TITLE = "Can Snoop Peek Vents";
        const string CAN_SNOOP_PEEKS_DESCRIPTION = "Whether you see things through walls when near \"peek\" vents";

        _snoopingCanSnoopPeeksBase = config.Bind(section, $"{CAN_SNOOP_PEEKS_TITLE} {LEVEL_1_TITLE}", false,
                                                    new ConfigDescription($"{CAN_SNOOP_PEEKS_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopPeeksFirst = config.Bind(section, $"{CAN_SNOOP_PEEKS_TITLE} {LEVEL_2_TITLE}", false,
                                                     new ConfigDescription($"{CAN_SNOOP_PEEKS_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopPeeksSecond = config.Bind(section, $"{CAN_SNOOP_PEEKS_TITLE} {LEVEL_3_TITLE}", false,
                                                      new ConfigDescription($"{CAN_SNOOP_PEEKS_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        const string CAN_SNOOP_SECURITY_TITLE = "Can Snoop Security Systems";
        const string CAN_SNOOP_SECURITY_DESCRIPTION = "Whether you see security systems through walls when near vents";
        
        _snoopingCanSnoopSecurityBase = config.Bind(section, $"{CAN_SNOOP_SECURITY_TITLE} {LEVEL_1_TITLE}", false,
                                                new ConfigDescription($"{CAN_SNOOP_SECURITY_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopSecurityFirst = config.Bind(section, $"{CAN_SNOOP_SECURITY_TITLE} {LEVEL_2_TITLE}", true,
                                                    new ConfigDescription($"{CAN_SNOOP_SECURITY_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopSecuritySecond = config.Bind(section, $"{CAN_SNOOP_SECURITY_TITLE} {LEVEL_3_TITLE}", true,
                                                     new ConfigDescription($"{CAN_SNOOP_SECURITY_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        const string CAN_PASS_TIME_NEAR_VENTS_TITLE = "Can Pass Time Near Vents";
        const string CAN_PASS_TIME_NEAR_VENTS_DESCRIPTION = "Whether you can stare at your watch to pass time when near vents";
        
        _snoopingCanPassTimeBase = config.Bind(section, $"{CAN_PASS_TIME_NEAR_VENTS_TITLE} {LEVEL_1_TITLE}", false,
                                                   new ConfigDescription($"{CAN_PASS_TIME_NEAR_VENTS_DESCRIPTION} {LEVEL_1_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanPassTimeFirst = config.Bind(section, $"{CAN_PASS_TIME_NEAR_VENTS_TITLE} {LEVEL_2_TITLE}", false,
                                              new ConfigDescription($"{CAN_PASS_TIME_NEAR_VENTS_DESCRIPTION} {LEVEL_2_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanPassTimeSecond = config.Bind(section, $"{CAN_PASS_TIME_NEAR_VENTS_TITLE} {LEVEL_3_TITLE}", true,
                                              new ConfigDescription($"{CAN_PASS_TIME_NEAR_VENTS_DESCRIPTION} {LEVEL_3_DESCRIPTION} {NAME_SHORT_SNOOPING}."));
        
        _snoopingPassTimeWarpDelay = config.Bind(section, "Pass Time Warp Delay", 10f,
                                             new ConfigDescription($"When you can pass time near vents, how long you must stare at your watch to pass time."));
        
        _snoopingPassTimeNotificationDelay = config.Bind(section, "Pass Time Notification Delay", 5f,
                                             new ConfigDescription($"When about to pass time near vents, when to notify you while staring at your watch that you are about to pass time. (Set to negative number for no notification.)"));
    }
    
    private static void CacheSnooping()
    {
        SnoopingPassTimeWarpDelay = new ConfigCacheSimple<float>(_snoopingPassTimeWarpDelay);
        SnoopingPassTimeNotificationDelay = new ConfigCacheSimple<float>(_snoopingPassTimeNotificationDelay);
        
        SnoopingCanSnoopCivilians = new ConfigCacheDiskEffect<bool>(false,
                                                          (level, oldValue, newValue) =>
                                                              $"You can now see unaware civilians through walls when in air ducts near vents.",
                                                          _snoopingCanSnoopCiviliansBase, _snoopingCanSnoopCiviliansFirst, _snoopingCanSnoopCiviliansSecond);
        
        SnoopingCanSnoopPeeks = new ConfigCacheDiskEffect<bool>(false,
                                                      (level, oldValue, newValue) =>
                                                          $"Snooping now also applies when near \"peeking\" vents in the middle of air ducts.",
                                                      _snoopingCanSnoopPeeksBase, _snoopingCanSnoopPeeksFirst, _snoopingCanSnoopPeeksSecond);
        
        SnoopingCanSnoopSecurity = new ConfigCacheDiskEffect<bool>(false,
                                                         (level, oldValue, newValue) =>
                                                             $"You can now see most security devices through walls when in air ducts near vents.",
                                                         _snoopingCanSnoopSecurityBase, _snoopingCanSnoopSecurityFirst, _snoopingCanSnoopSecuritySecond);
        
        SnoopingCanPassTime = new ConfigCacheDiskEffect<bool>(false,
                                                    (level, oldValue, newValue) =>
                                                        $"When in air ducts near vents, you can now stare at your watch to pass time faster.",
                                                    _snoopingCanPassTimeBase, _snoopingCanPassTimeFirst, _snoopingCanPassTimeSecond);
    }

    private static void ResetSnooping()
    {
        _snoopingPassTimeWarpDelay.Value = (float)_snoopingPassTimeWarpDelay.DefaultValue;
        _snoopingPassTimeNotificationDelay.Value = (float)_snoopingPassTimeNotificationDelay.DefaultValue;
        _snoopingCanSnoopCiviliansBase.Value = (bool)_snoopingCanSnoopCiviliansBase.DefaultValue;
        _snoopingCanSnoopCiviliansFirst.Value = (bool)_snoopingCanSnoopCiviliansFirst.DefaultValue;
        _snoopingCanSnoopCiviliansSecond.Value = (bool)_snoopingCanSnoopCiviliansSecond.DefaultValue;
        _snoopingCanSnoopPeeksBase.Value = (bool)_snoopingCanSnoopPeeksBase.DefaultValue;
        _snoopingCanSnoopPeeksFirst.Value = (bool)_snoopingCanSnoopPeeksFirst.DefaultValue;
        _snoopingCanSnoopPeeksSecond.Value = (bool)_snoopingCanSnoopPeeksSecond.DefaultValue;
        _snoopingCanSnoopSecurityBase.Value = (bool)_snoopingCanSnoopSecurityBase.DefaultValue;
        _snoopingCanSnoopSecurityFirst.Value = (bool)_snoopingCanSnoopSecurityFirst.DefaultValue;
        _snoopingCanSnoopSecuritySecond.Value = (bool)_snoopingCanSnoopSecuritySecond.DefaultValue;
        _snoopingCanPassTimeBase.Value = (bool)_snoopingCanPassTimeBase.DefaultValue;
        _snoopingCanPassTimeFirst.Value = (bool)_snoopingCanPassTimeFirst.DefaultValue;
        _snoopingCanPassTimeSecond.Value = (bool)_snoopingCanPassTimeSecond.DefaultValue;
    }
}