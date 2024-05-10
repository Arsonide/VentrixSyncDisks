using BepInEx.Configuration;

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

        _snoopingCanSnoopCiviliansBase = config.Bind(section, "Can Snoop Civilians (Base Level)", true,
                                                    new ConfigDescription($"Whether you see civilians through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopCiviliansFirst = config.Bind(section, "Can Snoop Civilians (First Upgrade)", true,
                                                     new ConfigDescription($"Whether you see civilians through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopCiviliansSecond = config.Bind(section, "Can Snoop Civilians (Second Upgrade)", true,
                                                      new ConfigDescription($"Whether you see civilians through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));

        _snoopingCanSnoopPeeksBase = config.Bind(section, "Can Snoop Peek Vents (Base Level)", false,
                                                    new ConfigDescription($"Whether you see things through walls when near \"peek\" vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopPeeksFirst = config.Bind(section, "Can Snoop Peek Vents (First Upgrade)", false,
                                                     new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopPeeksSecond = config.Bind(section, "Can Snoop Peek Vents (Second Upgrade)", false,
                                                      new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopSecurityBase = config.Bind(section, "Can Snoop Security Systems (Base Level)", false,
                                                new ConfigDescription($"Whether you see security systems through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopSecurityFirst = config.Bind(section, "Can Snoop Security Systems (First Upgrade)", true,
                                                    new ConfigDescription($"Whether you see security systems through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanSnoopSecuritySecond = config.Bind(section, "Can Snoop Security Systems (Second Upgrade)", true,
                                                     new ConfigDescription($"Whether you see security systems through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanPassTimeBase = config.Bind(section, "Can Pass Time Near Vents (Base Level)", false,
                                                   new ConfigDescription($"Whether you can stare at your watch to pass time when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanPassTimeFirst = config.Bind(section, "Can Pass Time Near Vents (First Upgrade)", false,
                                              new ConfigDescription($"Whether you can stare at your watch to pass time when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingCanPassTimeSecond = config.Bind(section, "Can Pass Time Near Vents (Second Upgrade)", true,
                                              new ConfigDescription($"Whether you can stare at your watch to pass time when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        _snoopingPassTimeWarpDelay = config.Bind(section, "Pass Time Warp Delay", 10f,
                                             new ConfigDescription($"When using {NAME_LONG_SNOOPING}, how long you must stare at your watch to pass time near vents."));
        
        _snoopingPassTimeNotificationDelay = config.Bind(section, "Pass Time Notification Delay", 5f,
                                             new ConfigDescription($"When using {NAME_LONG_SNOOPING}, when to notify you while staring at your watch that you are about to pass time near vents. (Set to negative number for no notification.)"));
    }
    
    private static void CacheSnooping()
    {
        SnoopingPassTimeWarpDelay = new ConfigCacheSimple<float>(_snoopingPassTimeWarpDelay);
        SnoopingPassTimeNotificationDelay = new ConfigCacheSimple<float>(_snoopingPassTimeNotificationDelay);
        
        SnoopingCanSnoopCivilians = new ConfigCacheDiskEffect<bool>(false,
                                                          (level, oldValue, newValue) =>
                                                              $"You can now see unaware civilians through walls when near vent entrances.",
                                                          _snoopingCanSnoopCiviliansBase, _snoopingCanSnoopCiviliansFirst, _snoopingCanSnoopCiviliansSecond);
        
        SnoopingCanSnoopPeeks = new ConfigCacheDiskEffect<bool>(false,
                                                      (level, oldValue, newValue) =>
                                                          $"Snooping now also applies when near \"peeking\" vents in the middle of air ducts.",
                                                      _snoopingCanSnoopPeeksBase, _snoopingCanSnoopPeeksFirst, _snoopingCanSnoopPeeksSecond);
        
        SnoopingCanSnoopSecurity = new ConfigCacheDiskEffect<bool>(false,
                                                         (level, oldValue, newValue) =>
                                                             $"You can now see cameras, laser sensors, sentry guns, and gas dispensers through walls when near vent entrances.",
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