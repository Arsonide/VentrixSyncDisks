//#define MOCK_INSTALLATION // Uncomment to pretend the Sync Disk is installed, even if it isn't.
#define EASILY_ATTAINABLE // Uncomment to make the Sync Disk free and at every location.

using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using SOD.Common.Helpers.SyncDiskObjects;
using VentVigilante.Hooks;

namespace VentVigilante.Implementation.Common;

public static class SyncDisks
{
    public const int MECHANIC_MULTIPLIER_1 = 3;
    public const int MECHANIC_MULTIPLIER_2 = 5;
    public const int MECHANIC_MULTIPLIER_3 = 7;
    
    public static bool IsMechanicInstalled => _mechanicLevel > 0;
    public static bool IsMapperInstalled => _mapperLevel > 0;
    public static bool IsCreatureInstalled => _creatureLevel > 0;
    public static int MechanicLevel => _mechanicLevel;
    public static int MapperLevel => _mapperLevel;
    public static int CreatureLevel => _creatureLevel;

    private static int _mechanicLevel = -1;
    private static int _mapperLevel = -1;
    private static int _creatureLevel = -1;

    private static int _mechanicEffectId = -1;
    private static int _mapperEffectId = -1;
    private static int _creatureEffectId = -1;

    private static HashSet<int> _mechanicUpgradeOptionIds = new HashSet<int>();
    private static HashSet<int> _mapperUpgradeOptionIds = new HashSet<int>();
    private static HashSet<int> _creatureUpgradeOptionIds = new HashSet<int>();

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
        Lib.SyncDisks.Builder("Vent Vigilante", "VentVigilante", reRaiseEventsOnSaveLoad: true)
           
#if EASILY_ATTAINABLE
           .SetPrice(0)
           .SetManufacturer(SyncDiskPreset.Manufacturer.ElGen)
           .SetRarity(SyncDiskPreset.Rarity.common)

           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.CigaretteMachine)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.AmericanDiner)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFridge)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.CoffeeMachine)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.StreetVendorSnacks)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.ElGenMachine)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.PawnShop)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.KolaMachine)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketMagazines)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.AmericanBar)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFruit)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Chinese)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Newsstand)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Supermarket)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Chemist)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Hardware)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketDrinksCooler)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketShelf)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.PoliceAutomat)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFreezer)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.HomeCoffee)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.NewspaperBox)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.WeaponsDealer)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SyncClinic)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketTrader)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketSyncClinic)
#else
           .SetPrice(1000)
           .SetManufacturer(SyncDiskPreset.Manufacturer.BlackMarket)
           .SetRarity(SyncDiskPreset.Rarity.medium)
           
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketTrader)
           .AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketSyncClinic)
#endif
           
           .AddEffect("Vent Mechanic", $"Vent crawling is now {MECHANIC_MULTIPLIER_1}x faster; you do not get cold in vents.", out int mechanicEffectId, "IconRun")
           .AddUpgradeOption(new SyncDiskBuilder.Options($"Vent crawling is now {MECHANIC_MULTIPLIER_2}x faster; vents close as you enter or leave them.",
                                                         $"Vent crawling is now {MECHANIC_MULTIPLIER_3}x faster; enter and exit vents further away."),
                             out SyncDiskBuilder.OptionIds mechanicUpgradeIds)
           
           .AddEffect("Vent Mapper", "Throwing a coin in vents will mark that vent for a while through walls; you do not make sounds when moving in vents.", out int mapperEffectId, "IconMap")
           .AddUpgradeOption(new SyncDiskBuilder.Options("Throwing a coin in vents will now also release an \"echolocation\" wave that briefly marks any connected vents.",
                                                         "Marks from echolocation waves now last as long as normal marks."),
                             out SyncDiskBuilder.OptionIds mapperUpgradeIds)
           
           .AddEffect("Vent Creature", "Throwing a coin in vents will mark that vent for a while through walls; you do not make sounds when moving in vents.", out int creatureEffectId, "IconMap")
           .AddUpgradeOption(new SyncDiskBuilder.Options("Throwing a coin in vents will now also release an \"echolocation\" wave that briefly marks any connected vents.",
                                                         "Marks from echolocation waves now last as long as normal marks."),
                             out SyncDiskBuilder.OptionIds creatureUpgradeIds)
           
           .CreateAndRegister();

        _mechanicEffectId = mechanicEffectId;
        _mapperEffectId = mapperEffectId;
        _creatureEffectId = creatureEffectId;

        _mechanicUpgradeOptionIds.Clear();
        _mechanicUpgradeOptionIds.Add(mechanicUpgradeIds.Option1Id);
        _mechanicUpgradeOptionIds.Add(mechanicUpgradeIds.Option2Id);
        
        _mapperUpgradeOptionIds.Clear();
        _mapperUpgradeOptionIds.Add(mapperUpgradeIds.Option1Id);
        _mapperUpgradeOptionIds.Add(mapperUpgradeIds.Option2Id);
        
        _creatureUpgradeOptionIds.Clear();
        _creatureUpgradeOptionIds.Add(creatureUpgradeIds.Option1Id);
        _creatureUpgradeOptionIds.Add(creatureUpgradeIds.Option2Id);
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
        _mechanicLevel = 0;
        _mapperLevel = 0;
        _creatureLevel = 0;
        OnLevelsModified();
    }
    
    private static void OnAfterSyncDiskInstalled(object sender, SyncDiskArgs args)
    {
        if (!args.Effect.HasValue)
        {
            return;
        }

        int id = args.Effect.Value.Id;

        if (_mechanicEffectId == id)
        {
            _mechanicLevel = 1;
            OnLevelsModified();
        }
        else if (_mapperEffectId == id)
        {
            _mapperLevel = 1;
            OnLevelsModified();
        }
        else if (_creatureEffectId == id)
        {
            _creatureLevel = 1;
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

        if (_mechanicUpgradeOptionIds.Contains(id))
        {
            _mechanicLevel++;
            OnLevelsModified();
        }
        else if (_mapperUpgradeOptionIds.Contains(id))
        {
            _mapperLevel++;
            OnLevelsModified();
        }
        else if (_creatureUpgradeOptionIds.Contains(id))
        {
            _creatureLevel++;
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

        if (_mechanicEffectId == id)
        {
            _mechanicLevel = 0;
            OnLevelsModified();
        }
        else if (_mapperEffectId == id)
        {
            _mapperLevel = 0;
            OnLevelsModified();
        }
        else if (_creatureEffectId == id)
        {
            _creatureLevel = 0;
            OnLevelsModified();
        }
    }

    private static void OnLevelsModified()
    {
        InteractableControllerSetupHook.RefreshVentInteractionRanges();
    }
    
    #endregion
}