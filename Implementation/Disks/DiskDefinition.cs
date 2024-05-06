#define EASILY_ATTAINABLE // Uncomment to make the Sync Disk free and at every location.

using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers.SyncDiskObjects;
using VentrixSyncDisks.Implementation.Config;

namespace VentrixSyncDisks.Implementation.Disks;

public class DiskDefinition
{
    public string Name = string.Empty;
    public int Price = 1000;
    public List<DiskEffectDefinition> Effects = new List<DiskEffectDefinition>();

    public void Register()
    {
        SyncDiskBuilder builder = Lib.SyncDisks.Builder(Name, MyPluginInfo.PLUGIN_GUID, true);
        builder.SetManufacturer(SyncDiskPreset.Manufacturer.KensingtonIndigo);

#if EASILY_ATTAINABLE
        builder.SetPrice(0);
        builder.SetRarity(SyncDiskPreset.Rarity.common);

        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.CigaretteMachine);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.AmericanDiner);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFridge);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.CoffeeMachine);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.StreetVendorSnacks);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.ElGenMachine);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.PawnShop);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.KolaMachine);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketMagazines);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.AmericanBar);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFruit);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Chinese);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Newsstand);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Supermarket);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Chemist);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.Hardware);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketDrinksCooler);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketShelf);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.PoliceAutomat);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SupermarketFreezer);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.HomeCoffee);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.NewspaperBox);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.WeaponsDealer);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SyncClinic);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketTrader);
        builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketSyncClinic);
#else
        builder.SetPrice(1000);
        builder.SetRarity(SyncDiskPreset.Rarity.medium);

        if (VentrixConfig.AvailableAtLegitSyncDiskClinics.Value)
        {
            builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.SyncClinic);
        }
        
        if (VentrixConfig.AvailableAtShadySyncDiskClinics.Value)
        {
            builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketSyncClinic);
        }

        if (VentrixConfig.AvailableAtBlackMarkets.Value)
        {
            builder.AddSaleLocation(SyncDiskBuilder.SyncDiskSaleLocation.BlackmarketTrader);
        }
#endif

        foreach (DiskEffectDefinition effect in Effects)
        {
            builder.AddEffect(effect.Name, effect.Description, out effect.EffectId, effect.Icon);
            SyncDiskBuilder.Options options = default(SyncDiskBuilder.Options);

            switch (effect.Upgrades.Count)
            {
                case 1:
                    options = new SyncDiskBuilder.Options(effect.Upgrades[0]);
                    break;
                case 2:
                    options = new SyncDiskBuilder.Options(effect.Upgrades[0], effect.Upgrades[1]);
                    break;
                case 3:
                    options = new SyncDiskBuilder.Options(effect.Upgrades[0], effect.Upgrades[1], effect.Upgrades[2]);
                    break;
            }

            builder.AddUpgradeOption(options, out effect.OptionIds);
        }

        builder.SetCanBeSideJobReward(true);
        builder.SetWorldSpawnOption(true);
        
        builder.CreateAndRegister();
    }
}