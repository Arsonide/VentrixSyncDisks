using BepInEx.Logging;
using UnityEngine;

namespace VentVigilante.Implementation.Common;

public static class Utilities
{
    public const bool DEBUG_BUILD = false;
    
    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        // Debug does not appear, presumably because it's for some functionality we don't have. We'll use it to filter based on DEBUG_BUILD instead.
        if (level == LogLevel.Debug)
        {
#pragma warning disable CS0162

            if (DEBUG_BUILD)
            {
                level = LogLevel.Info;
            }
            else
            {
                return;
            }

#pragma warning restore CS0162
        }

        VentVigilantePlugin.Log.Log(level, message);
    }

    public static int MultiplierForDescription(float multiplier, string lowerDescription, string higherDescription, out string description)
    {
        if (multiplier < 1)
        {
            description = lowerDescription;
            return Mathf.RoundToInt((1 - multiplier) * 100);
        }

        description = higherDescription;
        return Mathf.RoundToInt((multiplier - 1) * 100);
    }
}