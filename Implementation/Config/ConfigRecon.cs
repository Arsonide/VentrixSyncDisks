using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private static ConfigEntry<int> MappingEcholocationRangeBase;
    private static ConfigEntry<int> MappingEcholocationRangeFirst;
    private static ConfigEntry<int> MappingEcholocationRangeSecond;
    public static ConfigCache<int> MappingEcholocationRange;
    
    private static ConfigEntry<float> MappingEcholocationSpeedBase;
    private static ConfigEntry<float> MappingEcholocationSpeedFirst;
    private static ConfigEntry<float> MappingEcholocationSpeedSecond;
    public static ConfigCache<float> MappingEcholocationSpeed;

    private static ConfigEntry<float> MappingEcholocationDurationBase;
    private static ConfigEntry<float> MappingEcholocationDurationFirst;
    private static ConfigEntry<float> MappingEcholocationDurationSecond;
    public static ConfigCache<float> MappingEcholocationDuration;

    private static ConfigEntry<float> MappingCoinMultiplierBase;
    private static ConfigEntry<float> MappingCoinMultiplierFirst;
    private static ConfigEntry<float> MappingCoinMultiplierSecond;
    public static ConfigCache<float> MappingCoinMultiplier;
    
    public static ConfigEntry<float> MappingCentralNodeSize;
    public static ConfigEntry<bool> MappingUseDirectionalNodes;
    public static ConfigEntry<float> MappingDirectionalNodeLength;
    public static ConfigEntry<float> MappingDirectionalNodeDiameter;
    public static ConfigEntry<float> MappingDirectionalNodeOffset;

    public static ConfigEntry<string> MappingNodeColorNormal;
    public static ConfigEntry<string> MappingNodeColorVent;
    public static ConfigEntry<string> MappingNodeColorPeek;

    public static ConfigEntry<float> MappingNodeSpawnTime;
    public static ConfigEntry<float> MappingNodeDespawnTime;
    
    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansBase;
    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopCiviliansSecond;
    public static ConfigCache<bool> SnoopingCanSnoopCivilians;
    
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksBase;
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopPeeksSecond;
    public static ConfigCache<bool> SnoopingCanSnoopPeeks;
    
    private static ConfigEntry<bool> SnoopingCanSnoopSecurityBase;
    private static ConfigEntry<bool> SnoopingCanSnoopSecurityFirst;
    private static ConfigEntry<bool> SnoopingCanSnoopSecuritySecond;
    public static ConfigCache<bool> SnoopingCanSnoopSecurity;

    private static void InitializeRecon(ConfigFile config)
    {
        MappingEcholocationRangeBase = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (Base Level)", 10,
                                                   new ConfigDescription($"How far your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationRangeFirst = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (First Upgrade)", 20,
                                                    new ConfigDescription($"How far your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationRangeSecond = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Range (Second Upgrade)", 20,
                                                     new ConfigDescription($"How far your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedBase = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (Base Level)", 0.1f,
                                                   new ConfigDescription($"How quickly your echolocation pulse travels down vents with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedFirst = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (First Upgrade)", 0.1f,
                                                    new ConfigDescription($"How quickly your echolocation pulse travels down vents with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationSpeedSecond = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Speed (Second Upgrade)", 0.1f,
                                                     new ConfigDescription($"How quickly your echolocation pulse travels down vents with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationDurationBase = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (Base Level)", 1f,
                                                      new ConfigDescription($"How long it takes for your echolocation pulse to expire with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingEcholocationDurationFirst = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (First Upgrade)", 1f,
                                                       new ConfigDescription($"How long it takes for your echolocation pulse to expire with the first upgrade of {NAME_SHORT_MAPPING}."));

        MappingEcholocationDurationSecond = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Echolocation Duration (Second Upgrade)", 1f,
                                                        new ConfigDescription($"How long it takes for your echolocation pulse to expire with the second upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierBase = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (Base Level)", 1f,
                                                new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the base level of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierFirst = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (First Upgrade)", 1f,
                                                 new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the first upgrade of {NAME_SHORT_MAPPING}."));
        
        MappingCoinMultiplierSecond = config.Bind($"4. {NAME_SHORT_MAPPING} Disk", "Coin Duration Multiplier (Second Upgrade)", 0.1f,
                                                  new ConfigDescription($"Multiplier on echolocation duration while holding a coin with the second upgrade of {NAME_SHORT_MAPPING}."));

        MappingCentralNodeSize = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Central Node Size", 0.1f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how large the visualized nodes are in the center of each duct."));

        MappingUseDirectionalNodes = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Use Directional Nodes", true,
                                                 new ConfigDescription($"When using  {NAME_LONG_MAPPING}, whether to render additional indicators pointing at connected air ducts."));

        MappingDirectionalNodeLength = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Directional Node Length", 0.5f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how long the indicators pointing down connected ducts are."));

        MappingDirectionalNodeDiameter = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Directional Node Diameter", 0.015f,
                                                     new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how wide the indicators pointing down connected ducts are."));

        MappingDirectionalNodeOffset = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Directional Node Offset", 0.525f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how far apart the indicators pointing down connected ducts are."));

        MappingNodeColorNormal = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Node Color Normal", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color normal air ducts are visualized as."));

        MappingNodeColorVent = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Node Color Vent", "FFFF00",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color air vents (entrances / exits) are visualized as."));

        MappingNodeColorPeek = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Node Color Peek", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color \"peek\" ducts are visualized as, that you can see through but not exit."));
        
        MappingNodeSpawnTime = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Node Spawn Time", 0.2f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes spawn in, how long it takes for them to reach full size."));
        
        MappingNodeDespawnTime = config.Bind($"4. {NAME_SHORT_MAPPING} Rendering", "Node Despawn Time", 0.2f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes expire, how long it takes for them to shrink and disappear."));

        SnoopingCanSnoopCiviliansBase = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (Base Level)", true,
                                                    new ConfigDescription($"Whether you see civilians through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopCiviliansFirst = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (First Upgrade)", true,
                                                     new ConfigDescription($"Whether you see civilians through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopCiviliansSecond = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Civilians (Second Upgrade)", true,
                                                      new ConfigDescription($"Whether you see civilians through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));

        SnoopingCanSnoopPeeksBase = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (Base Level)", false,
                                                    new ConfigDescription($"Whether you see things through walls when near \"peek\" vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopPeeksFirst = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (First Upgrade)", true,
                                                     new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopPeeksSecond = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Peek Vents (Second Upgrade)", true,
                                                      new ConfigDescription($"Whether you see things through walls when near \"peek\" vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecurityBase = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (Base Level)", false,
                                                new ConfigDescription($"Whether you see security systems through walls when near vents at the base level of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecurityFirst = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (First Upgrade)", false,
                                                    new ConfigDescription($"Whether you see security systems through walls when near vents with the first upgrade of {NAME_SHORT_SNOOPING}."));
        
        SnoopingCanSnoopSecuritySecond = config.Bind($"6. {NAME_SHORT_SNOOPING}", "Can Snoop Security Systems (Second Upgrade)", true,
                                                     new ConfigDescription($"Whether you see security systems through walls when near vents with the second upgrade of {NAME_SHORT_SNOOPING}."));
        
        // Setup Caches
        MappingEcholocationRange = new ConfigCache<int>(-1,
                                                        GetEcholocationRangeDescription,
                                                        MappingEcholocationRangeBase, MappingEcholocationRangeFirst, MappingEcholocationRangeSecond);

        MappingEcholocationSpeed = new ConfigCache<float>(0.1f,
                                                          (level, oldValue, newValue) => $"Your \"echolocation\" pulses now travel {Utilities.MultiplierForDescription(newValue / oldValue, "slower", "faster", out string description)}% {description}.",
                                                          MappingEcholocationSpeedBase, MappingEcholocationSpeedFirst, MappingEcholocationSpeedSecond);

        MappingEcholocationDuration = new ConfigCache<float>(1f,
                                                             (level, oldValue, newValue) => $"Your \"echolocation\" pulses now expire {Utilities.MultiplierForDescription(newValue / oldValue, "faster", "slower", out string description)}% {description}.",
                                                             MappingEcholocationDurationBase, MappingEcholocationDurationFirst, MappingEcholocationDurationSecond);

        MappingCoinMultiplier = new ConfigCache<float>(1f,
                                                       (level, oldValue, newValue) =>
                                                           $"You remember your \"echolocation\" pulse {Utilities.MultiplierForDescription(newValue, "longer", "shorter", out string description)}% {description} while holding a coin.",
                                                 MappingCoinMultiplierBase, MappingCoinMultiplierFirst, MappingCoinMultiplierSecond);
        
        SnoopingCanSnoopCivilians = new ConfigCache<bool>(false,
                                                       (level, oldValue, newValue) =>
                                                           $"You can now see unaware civilians through walls when near vent entrances.",
                                                       SnoopingCanSnoopCiviliansBase, SnoopingCanSnoopCiviliansFirst, SnoopingCanSnoopCiviliansSecond);
        
        SnoopingCanSnoopPeeks = new ConfigCache<bool>(false,
                                                          (level, oldValue, newValue) =>
                                                              $"Snooping now also applies when near \"peeking\" vents in the middle of air ducts.",
                                                          SnoopingCanSnoopPeeksBase, SnoopingCanSnoopPeeksFirst, SnoopingCanSnoopPeeksSecond);
        
        SnoopingCanSnoopSecurity = new ConfigCache<bool>(false,
                                                      (level, oldValue, newValue) =>
                                                          $"You can now see cameras, laser sensors, sentry guns, and gas dispensers through walls when near vent entrances.",
                                                      SnoopingCanSnoopSecurityBase, SnoopingCanSnoopSecurityFirst, SnoopingCanSnoopSecuritySecond);
    }

    public static void ResetRecon()
    {
        MappingEcholocationRangeBase.Value = (int)MappingEcholocationRangeBase.DefaultValue;
        MappingEcholocationRangeFirst.Value = (int)MappingEcholocationRangeFirst.DefaultValue;
        MappingEcholocationRangeSecond.Value = (int)MappingEcholocationRangeSecond.DefaultValue;
        MappingEcholocationSpeedBase.Value = (float)MappingEcholocationSpeedBase.DefaultValue;
        MappingEcholocationSpeedFirst.Value = (float)MappingEcholocationSpeedFirst.DefaultValue;
        MappingEcholocationSpeedSecond.Value = (float)MappingEcholocationSpeedSecond.DefaultValue;
        MappingEcholocationDurationBase.Value = (float)MappingEcholocationDurationBase.DefaultValue;
        MappingEcholocationDurationFirst.Value = (float)MappingEcholocationDurationFirst.DefaultValue;
        MappingEcholocationDurationSecond.Value = (float)MappingEcholocationDurationSecond.DefaultValue;
        MappingCoinMultiplierBase.Value = (float)MappingCoinMultiplierBase.DefaultValue;
        MappingCoinMultiplierFirst.Value = (float)MappingCoinMultiplierFirst.DefaultValue;
        MappingCoinMultiplierSecond.Value = (float)MappingCoinMultiplierSecond.DefaultValue;
        SnoopingCanSnoopCiviliansBase.Value = (bool)SnoopingCanSnoopCiviliansBase.DefaultValue;
        SnoopingCanSnoopCiviliansFirst.Value = (bool)SnoopingCanSnoopCiviliansFirst.DefaultValue;
        SnoopingCanSnoopCiviliansSecond.Value = (bool)SnoopingCanSnoopCiviliansSecond.DefaultValue;
        SnoopingCanSnoopPeeksBase.Value = (bool)SnoopingCanSnoopPeeksBase.DefaultValue;
        SnoopingCanSnoopPeeksFirst.Value = (bool)SnoopingCanSnoopPeeksFirst.DefaultValue;
        SnoopingCanSnoopPeeksSecond.Value = (bool)SnoopingCanSnoopPeeksSecond.DefaultValue;
        SnoopingCanSnoopSecurityBase.Value = (bool)SnoopingCanSnoopSecurityBase.DefaultValue;
        SnoopingCanSnoopSecurityFirst.Value = (bool)SnoopingCanSnoopSecurityFirst.DefaultValue;
        SnoopingCanSnoopSecuritySecond.Value = (bool)SnoopingCanSnoopSecuritySecond.DefaultValue;
    }

    private static string GetEcholocationRangeDescription(int level, int oldValue, int newValue)
    {
        if (level == 1)
        {
            return "Throwing coins in air ducts will create brief \"echolocation\" pulses that show the vent network through walls.";
        }

        float change = (float)newValue / (float)oldValue;
        int percent = Utilities.MultiplierForDescription(change, "smaller", "larger", out string description);

        return $"Your \"echolocation\" pulses are now {percent}% {description}.";
    }
}