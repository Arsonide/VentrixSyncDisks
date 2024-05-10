using System.Collections.Generic;
using System.Text;
using SOD.Common;
using SOD.Common.Helpers;
using SOD.Common.Helpers.SyncDiskObjects;
using VentrixSyncDisks.Hooks;
using VentrixSyncDisks.Implementation.Config;

namespace VentrixSyncDisks.Implementation.Disks;

public static class DiskRegistry
{
    // These could be a dictionary but I feel that would slow down access for no reason.
    public static DiskCache ScootingDisk;
    public static DiskCache ParkourDisk;
    public static DiskCache MappingDisk;
    public static DiskCache SnoopingDisk;
    public static DiskCache SpecterDisk;
    public static DiskCache TerrorDisk;

    private static List<DiskCache> AllDisks = new List<DiskCache>();
    private static List<string> CacheDescriptions = new List<string>();
    private static StringBuilder Builder = new StringBuilder();
    
    public static void Initialize()
    {
        Register();
        SetEvents(true);
    }

    public static void Uninitialize()
    {
        SetEvents(false);
    }

    private static void Register()
    {
        AllDisks.Clear();

        if (VentrixConfig.MobilityEnabled.Value)
        {
            RegisterMobility();
            AllDisks.Add(ScootingDisk);
            AllDisks.Add(ParkourDisk);
        }

        if (VentrixConfig.ReconEnabled.Value)
        {
            RegisterRecon();
            AllDisks.Add(MappingDisk);
            AllDisks.Add(SnoopingDisk);
        }

        if (VentrixConfig.MischiefEnabled.Value)
        {
            RegisterMischief();
            AllDisks.Add(SpecterDisk);
            AllDisks.Add(TerrorDisk);
        }
    }

    private static string GetDiskLevelDescription(int level, params ConfigCache[] caches)
    {
        CacheDescriptions.Clear();

        foreach (ConfigCache cache in caches)
        {
            if (cache.GetDescriptionRelevant(level))
            {
                CacheDescriptions.Add(cache.GetDescription(level));
            }
        }

        int count = CacheDescriptions.Count;

        if (count <= 0)
        {
            return "Error Getting Description";
        }
        
        Builder.Clear();

        for (int i = 0; i < count; ++i)
        {
            bool isFirst = i == 0;
            bool isLast = i == count - 1;
            string raw = CacheDescriptions[i];
            Builder.Append(isFirst ? raw[0] : char.ToLowerInvariant(raw[0]));
            Builder.Append(raw, 1, raw.Length - 2);
            Builder.Append(isLast ? "." : "; ");
        }

        return Builder.ToString();
    }
    
    #region Level Management Events

    private static void SetEvents(bool registered)
    {
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SyncDisks.OnAfterSyncDiskInstalled -= OnAfterSyncDiskInstalled;
        Lib.SyncDisks.OnAfterSyncDiskUpgraded -= OnAfterSyncDiskUpgraded;
        Lib.SyncDisks.OnAfterSyncDiskUninstalled -= OnAfterSyncDiskUninstalled;

        if (registered)
        {
            Lib.SaveGame.OnAfterLoad += OnAfterLoad;
            Lib.SyncDisks.OnAfterSyncDiskInstalled += OnAfterSyncDiskInstalled;
            Lib.SyncDisks.OnAfterSyncDiskUpgraded += OnAfterSyncDiskUpgraded;
            Lib.SyncDisks.OnAfterSyncDiskUninstalled += OnAfterSyncDiskUninstalled;
        }
    }
    
    private static void OnAfterLoad(object sender, SaveGameArgs e)
    {
        foreach (DiskCache disk in AllDisks)
        {
            disk.Reset();
        }
        
        OnLevelsModified();
    }
    
    private static void OnAfterSyncDiskInstalled(object sender, SyncDiskArgs args)
    {
        if (!args.Effect.HasValue)
        {
            return;
        }

        int id = args.Effect.Value.Id;
        bool dirty = false;
        
        foreach (DiskCache disk in AllDisks)
        {
            if (disk.Install(id))
            {
                dirty = true;
            }
        }

        if (dirty)
        {
            OnLevelsModified();
        }
    }
    
    private static void OnAfterSyncDiskUpgraded(object sender, SyncDiskArgs args)
    {
        if (!args.UpgradeOption.HasValue)
        {
            return;
        }

        int id = args.UpgradeOption.Value.Id;
        bool dirty = false;
        
        foreach (DiskCache disk in AllDisks)
        {
            if (disk.Upgrade(id))
            {
                dirty = true;
            }
        }

        if (dirty)
        {
            OnLevelsModified();
        }
    }

    private static void OnAfterSyncDiskUninstalled(object sender, SyncDiskArgs args)
    {
        if (!args.Effect.HasValue)
        {
            return;
        }

        int id = args.Effect.Value.Id;
        bool dirty = false;
        
        foreach (DiskCache disk in AllDisks)
        {
            if (disk.Uninstall(id))
            {
                dirty = true;
            }
        }

        if (dirty)
        {
            OnLevelsModified();
        }
    }

    private static void OnLevelsModified()
    {
        ParkourHooks.InteractableControllerSetupHook.RefreshVentInteractionRanges();
    }
    
    #endregion
    
    #region Mobility Disk

    private static void RegisterMobility()
    {
        // Disk Properties
        DiskDefinition mobilityDisk = new DiskDefinition
        {
            Name = "Vent Mobility",
        };

        // Effect A Properties
        DiskEffectDefinition scootingEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_SCOOTING,
            Icon = "IconRun",
        };

        scootingEffect.Description = GetDiskLevelDescription(1, VentrixConfig.ScootingSpeedMultiplier);
        scootingEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.ScootingSpeedMultiplier));
        scootingEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.ScootingSpeedMultiplier));
        
        // Effect B Properties
        DiskEffectDefinition parkourEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_PARKOUR,
            Icon = "IconWetFloor",
        };

        parkourEffect.Description = GetDiskLevelDescription(1, VentrixConfig.ParkourTransitionSpeed, VentrixConfig.ParkourInteractRange, VentrixConfig.ParkourAutoClose);
        parkourEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.ParkourTransitionSpeed, VentrixConfig.ParkourInteractRange, VentrixConfig.ParkourAutoClose));
        parkourEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.ParkourTransitionSpeed, VentrixConfig.ParkourInteractRange, VentrixConfig.ParkourAutoClose));
        
        // Finishing Up
        mobilityDisk.Effects.Add(scootingEffect);
        mobilityDisk.Effects.Add(parkourEffect);

        mobilityDisk.Register();

        ScootingDisk = new DiskCache(scootingEffect.EffectId, scootingEffect.OptionIds);
        ParkourDisk = new DiskCache(parkourEffect.EffectId, parkourEffect.OptionIds);
    }
    
    #endregion
    
    #region Recon Disk

    private static void RegisterRecon()
    {
        // Disk Properties
        DiskDefinition reconDisk = new DiskDefinition
        {
            Name = "Vent Recon",
        };

        // Effect A Properties
        DiskEffectDefinition mappingEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_MAPPING,
            Icon = "IconCoinDistraction",
        };

        mappingEffect.Description = GetDiskLevelDescription(1, VentrixConfig.MappingEcholocationRange, VentrixConfig.MappingEcholocationSpeed, VentrixConfig.MappingEcholocationDuration, VentrixConfig.MappingCoinMultiplier);
        mappingEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.MappingEcholocationRange, VentrixConfig.MappingEcholocationSpeed, VentrixConfig.MappingEcholocationDuration, VentrixConfig.MappingCoinMultiplier));
        mappingEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.MappingEcholocationRange, VentrixConfig.MappingEcholocationSpeed, VentrixConfig.MappingEcholocationDuration, VentrixConfig.MappingCoinMultiplier));
        
        // Effect B Properties
        DiskEffectDefinition snoopingEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_SNOOPING,
            Icon = "IconOpticalCammo",
        };

        snoopingEffect.Description = GetDiskLevelDescription(1, VentrixConfig.SnoopingCanSnoopCivilians, VentrixConfig.SnoopingCanSnoopSecurity, VentrixConfig.SnoopingCanPassTime, VentrixConfig.SnoopingCanSnoopPeeks);
        snoopingEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.SnoopingCanSnoopCivilians, VentrixConfig.SnoopingCanSnoopSecurity, VentrixConfig.SnoopingCanPassTime, VentrixConfig.SnoopingCanSnoopPeeks));
        snoopingEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.SnoopingCanSnoopCivilians, VentrixConfig.SnoopingCanSnoopSecurity, VentrixConfig.SnoopingCanPassTime, VentrixConfig.SnoopingCanSnoopPeeks));

        // Finishing Up
        reconDisk.Effects.Add(mappingEffect);
        reconDisk.Effects.Add(snoopingEffect);

        reconDisk.Register();

        MappingDisk = new DiskCache(mappingEffect.EffectId, mappingEffect.OptionIds);
        SnoopingDisk = new DiskCache(snoopingEffect.EffectId, snoopingEffect.OptionIds);
    }

    #endregion

    #region Mischief Disk

    private static void RegisterMischief()
    {
        // Disk Properties
        DiskDefinition mischiefDisk = new DiskDefinition
        {
            Name = "Vent Mischief",
        };

        // Effect A Properties
        DiskEffectDefinition specterEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_SPECTER,
            Icon = "IconDeath",
        };

        specterEffect.Description = GetDiskLevelDescription(1, VentrixConfig.SpecterFootstepChance, VentrixConfig.SpecterColdImmunity);
        specterEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.SpecterFootstepChance, VentrixConfig.SpecterColdImmunity));
        specterEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.SpecterFootstepChance, VentrixConfig.SpecterColdImmunity));
        
        // Effect B Properties
        DiskEffectDefinition terrorEffect = new DiskEffectDefinition
        {
            Name = VentrixConfig.NAME_LONG_TERROR,
            Icon = "IconPassedOut",
        };

        terrorEffect.Description = GetDiskLevelDescription(1, VentrixConfig.TerrorFreakoutDuration, VentrixConfig.TerrorToxicImmunity);
        terrorEffect.Upgrades.Add(GetDiskLevelDescription(2, VentrixConfig.TerrorFreakoutDuration, VentrixConfig.TerrorToxicImmunity));
        terrorEffect.Upgrades.Add(GetDiskLevelDescription(3, VentrixConfig.TerrorFreakoutDuration, VentrixConfig.TerrorToxicImmunity));
        
        // Finishing Up
        mischiefDisk.Effects.Add(specterEffect);
        mischiefDisk.Effects.Add(terrorEffect);

        mischiefDisk.Register();

        SpecterDisk = new DiskCache(specterEffect.EffectId, specterEffect.OptionIds);
        TerrorDisk = new DiskCache(terrorEffect.EffectId, terrorEffect.OptionIds);
    }

    #endregion
}