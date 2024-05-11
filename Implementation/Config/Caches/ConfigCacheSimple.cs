using BepInEx.Configuration;

namespace VentrixSyncDisks.Implementation.Config.Caches;

public class ConfigCacheSimple<T>
{
    public readonly ConfigEntry<T> Entry;
    public readonly T Value;
    public readonly T DefaultValue;

    public ConfigCacheSimple(ConfigEntry<T> entry)
    {
        Entry = entry;
        Value = entry.Value;
        DefaultValue = (T)entry.DefaultValue;
    }
}