using BepInEx.Logging;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Common;

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

        VentrixPlugin.Log.Log(level, message);
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
    
    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length != 6 && hex.Length != 8)
        {
            return Color.white;
        }

        byte r = 255, g = 255, b = 255, a = 255;

        if (!byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out r) ||
            !byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out g) ||
            !byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out b))
        {
            return Color.white;
        }

        if (hex.Length == 8 && !byte.TryParse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null, out a))
        {
            return Color.white;
        }

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
    
    public static bool RoomsEqual(NewRoom a, NewRoom b)
    {
        bool aNull = a == null;
        bool bNull = b == null;
        
        if (aNull && bNull)
        {
            return false;
        }

        int aID = aNull ? -1 : a.GetInstanceID();
        int bID = bNull ? -1 : b.GetInstanceID();
        
        return aID == bID;
    }
}