using System.Collections.Generic;
using SOD.Common.Helpers.SyncDiskObjects;

namespace VentVigilante.Implementation.Disks;

public class VentrixDiskEffectDefinition
{
    public string Name = string.Empty;
    public string Icon = string.Empty;
    public string Description = string.Empty;

    public List<string> Upgrades = new List<string>();

    public int EffectId = -1;
    public SyncDiskBuilder.OptionIds OptionIds;
}