// #define MOCK_INSTALLATION // Uncomment to pretend the Sync Disk is installed, even if it isn't.

using System.Collections.Generic;
using SOD.Common.Helpers.SyncDiskObjects;

namespace VentrixSyncDisks.Implementation.Disks;

public class DiskCache
{
    public bool IsInstalled
    {
        get
        {
            return Level > 0;
        }
    }

    public int Level
    {
        get
        {
#pragma warning disable 0162
            
#if MOCK_INSTALLATION
            return _maxLevel;
#endif
            
            return _level;
            
#pragma warning restore 0162
        }
    }

    private readonly int _effectId;
    private readonly HashSet<int> _upgradeOptionIds;
    
    private int _level = -1;
    private int _maxLevel = -1;
    
    public DiskCache(int effect, SyncDiskBuilder.OptionIds options)
    {
        _effectId = effect;
        _upgradeOptionIds = new HashSet<int>();
        _maxLevel = 1;
        
        if (options.Option1Id != 0)
        {
            _upgradeOptionIds.Add(options.Option1Id);
            _maxLevel++;
        }
        
        if (options.Option2Id != 0)
        {
            _upgradeOptionIds.Add(options.Option2Id);
            _maxLevel++;
        }
        
        if (options.Option3Id != 0)
        {
            _upgradeOptionIds.Add(options.Option3Id);
            _maxLevel++;
        }
        
        Reset();
    }

    public void Reset()
    {
        _level = 0;
    }
    
    public bool Install(int id)
    {
        if (id != _effectId)
        {
            return false;
        }

        _level = 1;
        return true;
    }
    
    public bool Uninstall(int id)
    {
        if (id != _effectId)
        {
            return false;
        }

        _level = 0;
        return true;
    }
    
    public bool Upgrade(int id)
    {
        if (!_upgradeOptionIds.Contains(id))
        {
            return false;
        }

        _level++;
        return true;
    }
}