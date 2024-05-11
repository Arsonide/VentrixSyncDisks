using System.Collections.Generic;
using SOD.Common.Helpers.SyncDiskObjects;

namespace VentrixSyncDisks.Implementation.Disks;

public class DiskEffectDefinition
{
    public string Name = string.Empty;
    public string Icon = string.Empty;
    public string Description = string.Empty;

    public readonly List<string> Upgrades = new List<string>();

    public int EffectId = -1;
    public SyncDiskBuilder.OptionIds OptionIds;
}