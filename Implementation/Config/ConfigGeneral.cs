using BepInEx.Configuration;
using BepInEx.Logging;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private const string ExpectedVersion = "af2cfc5492bd478c9e7ccda801b1151c";
    
    private const string LEVEL_1_TITLE = "(No Upgrades)";
    private const string LEVEL_2_TITLE = "(First Upgrade)";
    private const string LEVEL_3_TITLE = "(Second Upgrade)";

    private const string LEVEL_1_DESCRIPTION = "with no upgrades in";
    private const string LEVEL_2_DESCRIPTION = "with the first upgrade of";
    private const string LEVEL_3_DESCRIPTION = "with the second upgrade of";

    private const string NAME_SHORT_GENERAL = "General";
    private const int ID_GENERAL = 1;

    // These aren't used often and they are needed immediately so I'm not tangling them up in caches.
    public static ConfigEntry<bool> Enabled;
    public static ConfigEntry<string> Version;
    
    public static ConfigCacheSimple<bool> MobilityEnabled;
    public static ConfigCacheSimple<bool> ReconEnabled;
    public static ConfigCacheSimple<bool> MischiefEnabled;
    
    public static ConfigCacheSimple<bool> AvailableAtLegitSyncDiskClinics;
    public static ConfigCacheSimple<bool> AvailableAtShadySyncDiskClinics;
    public static ConfigCacheSimple<bool> AvailableAtBlackMarkets;
    
    private static ConfigEntry<bool> _mobilityEnabled;
    private static ConfigEntry<bool> _reconEnabled;
    private static ConfigEntry<bool> _mischiefEnabled;

    private static ConfigEntry<bool> _availableAtLegitSyncDiskClinics;
    private static ConfigEntry<bool> _availableAtShadySyncDiskClinics;
    private static ConfigEntry<bool> _availableAtBlackMarkets;

    public static void Initialize(ConfigFile config)
    {
        string section = $"{ID_GENERAL}. {NAME_SHORT_GENERAL}";
        
        Enabled = config.Bind(section, "Enabled", true,
                              new ConfigDescription("Another method of enabling and disabling Ventrix Sync Disks."));

        Version = config.Bind(section, "Version", string.Empty,
                              new ConfigDescription("Ventrix Sync Disks uses this to reset your configuration between major versions. Don't modify it or it will reset your configuration!"));

        _mobilityEnabled = config.Bind(section, "Vent Mobility Enabled", true,
                                       new ConfigDescription("Whether the \"Vent Mobility\" sync disk is in the game."));
        
        _reconEnabled = config.Bind(section, "Vent Recon Enabled", true,
                                    new ConfigDescription("Whether the \"Vent Recon\" sync disk is in the game."));
        
        _mischiefEnabled = config.Bind(section, "Vent Mischief Enabled", true,
                                       new ConfigDescription("Whether the \"Vent Mischief\" sync disk is in the game."));

        _availableAtLegitSyncDiskClinics = config.Bind(section, "Available At Legit Sync Disk Clinics", false,
                                                       new ConfigDescription("Whether Ventrix Industries sync disks are sold at legitimate sync disk clinics."));
        
        _availableAtShadySyncDiskClinics = config.Bind(section, "Available At Shady Sync Disk Clinics", true,
                                                       new ConfigDescription("Whether Ventrix Industries sync disks are sold at black market sync disk clinics."));
        
        _availableAtBlackMarkets = config.Bind(section, "Available At Black Markets", false,
                                               new ConfigDescription("Whether Ventrix Industries sync disks are sold at black markets."));
        
        InitializeScooting(config);
        InitializeParkour(config);
        InitializeMapping(config);
        InitializeSnooping(config);
        InitializeSpecter(config);
        InitializeTerror(config);
        InitializeRendering(config);
        
        ProcessUpgrades();

        CacheGeneral();
        CacheScooting();
        CacheParkour();
        CacheMapping();
        CacheSnooping();
        CacheSpecter();
        CacheTerror();
        CacheRendering();

        Utilities.Log("VentrixConfig has initialized!", LogLevel.Debug);
    }

    private static void ProcessUpgrades()
    {
        if (Version.Value == ExpectedVersion)
        {
            return;
        }

        Utilities.Log("Detected either a new installation or a major upgrade of Ventrix Sync Disks, resetting the configuration file!");
        Version.Value = ExpectedVersion;
        Reset();
    }

    private static void CacheGeneral()
    {
        MobilityEnabled = new ConfigCacheSimple<bool>(_mobilityEnabled);
        ReconEnabled = new ConfigCacheSimple<bool>(_reconEnabled);
        MischiefEnabled = new ConfigCacheSimple<bool>(_mischiefEnabled);
        AvailableAtLegitSyncDiskClinics = new ConfigCacheSimple<bool>(_availableAtLegitSyncDiskClinics);
        AvailableAtShadySyncDiskClinics = new ConfigCacheSimple<bool>(_availableAtShadySyncDiskClinics);
        AvailableAtBlackMarkets = new ConfigCacheSimple<bool>(_availableAtBlackMarkets);
    }

    private static void Reset()
    {
        Enabled.Value = (bool)Enabled.DefaultValue;
        _mobilityEnabled.Value = (bool)_mobilityEnabled.DefaultValue;
        _reconEnabled.Value = (bool)_reconEnabled.DefaultValue;
        _mischiefEnabled.Value = (bool)_mischiefEnabled.DefaultValue;
        _availableAtLegitSyncDiskClinics.Value = (bool)_availableAtLegitSyncDiskClinics.DefaultValue;
        _availableAtShadySyncDiskClinics.Value = (bool)_availableAtShadySyncDiskClinics.DefaultValue;
        _availableAtBlackMarkets.Value = (bool)_availableAtBlackMarkets.DefaultValue;

        ResetScooting();
        ResetParkour();
        
        ResetMapping();
        ResetSnooping();
        
        ResetSpecter();
        ResetTerror();
        
        ResetRendering();
    }
}