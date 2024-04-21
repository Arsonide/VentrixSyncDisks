using BepInEx.Configuration;
using BepInEx.Logging;
using VentVigilante.Implementation.Common;
using Il2CppSystem.IO;

namespace VentVigilante.Implementation.Config;

public static partial class VentVigilanteConfig
{
    private const string ExpectedVersion = "c031debd1e0f4d74a319ab0ada3f106b";
    
    public static ConfigEntry<string> Version;

    public static ConfigEntry<bool> Enabled;

    public static void Initialize(ConfigFile config)
    {
        Enabled = config.Bind("1. General", "Enabled", true,
                              new ConfigDescription("Another method of enabling and disabling Vent Vigilante."));

        Version = config.Bind("1. General", "Version", string.Empty,
                              new ConfigDescription("Vent Vigilante uses this to reset your configuration between major versions. Don't modify it or it will reset your configuration!"));

        ProcessUpgrades();
        
        Utilities.Log("VentVigilanteConfig has initialized!", LogLevel.Debug);
    }

    private static void ProcessUpgrades()
    {
        if (Version.Value == ExpectedVersion)
        {
            return;
        }

        Utilities.Log("Detected either a new installation or a major upgrade of Vent Vigilante, resetting the configuration file!");
        Version.Value = ExpectedVersion;
        Reset();
    }

    private static void Reset()
    {
        Enabled.Value = (bool)Enabled.DefaultValue;
    }
}