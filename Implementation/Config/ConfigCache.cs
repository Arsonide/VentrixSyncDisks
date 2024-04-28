using System;
using BepInEx.Configuration;

namespace VentVigilante.Implementation.Config;

public class ConfigCache<T> where T : IEquatable<T>
{
    private T[] _cachedLevels;
    private int _maxLevel;
    
    private Func<int, T, T, string> _descriptionDelegate;
    
    public ConfigCache(T defaultValue, Func<int, T, T, string> descriptionDelegate, params ConfigEntry<T>[] levels)
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
    
    public bool GetDescriptionRelevant(int level)
    {
        if (level < 1 || level > _maxLevel)
        {
            return false;
        }

        T thisValue = GetLevel(level);
        T oldValue = GetLevel(level - 1);
        
        return !thisValue.Equals(oldValue);
    }
    
    public string GetDescription(int level)
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