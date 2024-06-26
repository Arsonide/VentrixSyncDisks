﻿using System;
using BepInEx.Configuration;

namespace VentrixSyncDisks.Implementation.Config.Caches;

public abstract class ConfigCacheDiskEffect
{
    public abstract bool GetDescriptionRelevant(int level);
    public abstract string GetDescription(int level);
}

public class ConfigCacheDiskEffect<T> : ConfigCacheDiskEffect where T : IEquatable<T>
{
    private readonly T[] _cachedLevels;
    private readonly int _maxLevel;
    
    private readonly Func<int, T, T, string> _descriptionDelegate;
    
    public ConfigCacheDiskEffect(T defaultValue, Func<int, T, T, string> descriptionDelegate, params ConfigEntry<T>[] levels)
    {
        int levelCount = levels.Length;
        _cachedLevels = new T[levelCount + 1];
        _cachedLevels[0] = defaultValue;

        for (int i = 0; i < levelCount; ++i)
        {
            _cachedLevels[i + 1] = levels[i].Value;
        }

        _maxLevel = _cachedLevels.Length - 1;
        _descriptionDelegate = descriptionDelegate;
    }

    public T GetLevel(int level)
    {
        if (level < 0 || level > _maxLevel)
        {
            return _cachedLevels[0];
        }

        return _cachedLevels[level];
    }
    
    public override bool GetDescriptionRelevant(int level)
    {
        if (level < 1 || level > _maxLevel)
        {
            return false;
        }

        T thisValue = GetLevel(level);
        T oldValue = GetLevel(level - 1);
        
        return !thisValue.Equals(oldValue);
    }
    
    public override string GetDescription(int level)
    {
        if (level < 1 || level > _maxLevel)
        {
            return string.Empty;
        }

        T newValue = GetLevel(level);
        T oldValue = GetLevel(level - 1);
        
        return _descriptionDelegate?.Invoke(level, oldValue, newValue);
    }
}