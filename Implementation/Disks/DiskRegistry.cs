using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using SOD.Common.Helpers.SyncDiskObjects;
using VentVigilante.Hooks;

namespace VentVigilante.Implementation.Disks;

public static class DiskDatabase
{
    public const int MECHANIC_MULTIPLIER_1 = 3;
    public const int MECHANIC_MULTIPLIER_2 = 5;
    public const int MECHANIC_MULTIPLIER_3 = 7;

    // These could be a dictionary but I feel that would slow down access for no reason.
    public static DiskCache RunnerDisk;
    public static DiskCache ParkourDisk;
    public static DiskCache MappingDisk;
    public static DiskCache SnoopingDisk;
    public static DiskCache SpecterDisk;
    public static DiskCache MenaceDisk;

    private static List<DiskCache> AllDisks = new List<DiskCache>();
    
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
        RegisterMobility();
        RegisterRecon();
        RegisterMischief();

        AllDisks.Clear();
        AllDisks.Add(RunnerDisk);
        AllDisks.Add(ParkourDisk);
        AllDisks.Add(MappingDisk);
        AllDisks.Add(SnoopingDisk);
        AllDisks.Add(SpecterDisk);
        AllDisks.Add(MenaceDisk);
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
        InteractableControllerSetupHook.RefreshVentInteractionRanges();
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
        DiskEffectDefinition runnerEffect = new DiskEffectDefinition
        {
            Name = "Airway Runner",
            Icon = "IconRun",
        };

        runnerEffect.Description = "You now move a little faster through vents.";
        runnerEffect.Upgrades.Add("You now move moderately faster through vents.");
        runnerEffect.Upgrades.Add("You now move much faster through vents.");
        
        // Effect B Properties
        DiskEffectDefinition parkourEffect = new DiskEffectDefinition
        {
            Name = "Ductwork Parkour",
            Icon = "IconWetFloor",
        };

        parkourEffect.Description = "You enter and exit vents faster.";
        parkourEffect.Upgrades.Add("You can reach vents further away.");
        parkourEffect.Upgrades.Add("Vents automatically close when entering or exiting them.");
        
        // Finishing Up
        mobilityDisk.Effects.Add(runnerEffect);
        mobilityDisk.Effects.Add(parkourEffect);

        mobilityDisk.Register();

        RunnerDisk = new DiskCache(runnerEffect.EffectId, runnerEffect.OptionIds);
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
            Name = "Acoustic Mapping",
            Icon = "IconCoinDistraction",
        };

        mappingEffect.Description = "Throwing coins in air ducts will create brief \"echolocation\" pulses that show the vent network through walls.";
        mappingEffect.Upgrades.Add("Your \"echolocation\" pulses now cover a much wider area.");
        mappingEffect.Upgrades.Add("You now remember your \"echolocation\" pulses and see them as long as you are holding your coin.");
        
        // Effect B Properties
        DiskEffectDefinition snoopingEffect = new DiskEffectDefinition
        {
            Name = "Grill Snooping",
            Icon = "IconOpticalCammo",
        };

        snoopingEffect.Description = "When near vent entrances and exits you mark citizens in that room through walls.";
        snoopingEffect.Upgrades.Add("Your marks now also appear when near \"peeking\" vents in air ducts.");
        snoopingEffect.Upgrades.Add("Your marks now also apply to any security devices in that room.");
        
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
            Name = "Crawlspace Specter",
            Icon = "IconDeath",
        };

        specterEffect.Description = "You now make less noise when moving through air ducts.";
        specterEffect.Upgrades.Add("You no longer get cold when in vents.");
        specterEffect.Upgrades.Add("You now rarely make noise when moving through air ducts.");
        
        // Effect B Properties
        DiskEffectDefinition menaceEffect = new DiskEffectDefinition
        {
            Name = "Shaft Menace",
            Icon = "IconPassedOut",
        };

        menaceEffect.Description = "Citizens witnessing you burst forth from a vent can become frightened and flee.";
        menaceEffect.Upgrades.Add("You are no longer affected by toxic gas in vents.");
        menaceEffect.Upgrades.Add("Citizens are much more likely to flee when you exit vents.");
        
        // Finishing Up
        mischiefDisk.Effects.Add(specterEffect);
        mischiefDisk.Effects.Add(menaceEffect);

        mischiefDisk.Register();

        SpecterDisk = new DiskCache(specterEffect.EffectId, specterEffect.OptionIds);
        MenaceDisk = new DiskCache(menaceEffect.EffectId, menaceEffect.OptionIds);
    }

    #endregion
}