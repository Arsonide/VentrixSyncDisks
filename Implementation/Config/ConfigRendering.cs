using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private const int ID_RENDERING = 4;

    public static ConfigEntry<float> MappingCentralNodeSize;
    public static ConfigEntry<bool> MappingUseDirectionalNodes;
    public static ConfigEntry<bool> MappingSpecialDirectionalNodeColors;
    public static ConfigEntry<float> MappingDirectionalNodeLength;
    public static ConfigEntry<float> MappingDirectionalNodeDiameter;
    public static ConfigEntry<float> MappingDirectionalNodeOffset;

    public static ConfigEntry<string> MappingNodeColorNormal;
    public static ConfigEntry<string> MappingNodeColorVent;
    public static ConfigEntry<string> MappingNodeColorPeek;

    public static ConfigEntry<float> MappingNodeSpawnTime;
    public static ConfigEntry<float> MappingNodeDespawnTime;

    private static void InitializeRendering(ConfigFile config)
    {
        MappingCentralNodeSize = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Central Node Size", 0.1f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how large the visualized nodes are in the center of each duct."));

        MappingUseDirectionalNodes = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Use Directional Nodes", true,
                                                 new ConfigDescription($"When using  {NAME_LONG_MAPPING}, whether to render additional indicators pointing at connected air ducts."));

        MappingSpecialDirectionalNodeColors = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Special Directional Node Colors", false,
                                                 new ConfigDescription($"When using  {NAME_LONG_MAPPING}, whether additional connections on vents are colored as normal ducts or as vents."));
        
        MappingDirectionalNodeLength = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Length", 0.5f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how long the indicators pointing down connected ducts are."));

        MappingDirectionalNodeDiameter = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Diameter", 0.015f,
                                                     new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how wide the indicators pointing down connected ducts are."));

        MappingDirectionalNodeOffset = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Offset", 0.525f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how far apart the indicators pointing down connected ducts are."));

        MappingNodeColorNormal = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Normal", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color normal air ducts are visualized as."));

        MappingNodeColorVent = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Vent", "FFFF00",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color air vents (entrances / exits) are visualized as."));

        MappingNodeColorPeek = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Peek", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color \"peek\" ducts are visualized as, that you can see through but not exit."));
        
        MappingNodeSpawnTime = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Spawn Time", 0.2f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes spawn in, how long it takes for them to reach full size."));
        
        MappingNodeDespawnTime = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Despawn Time", 0.2f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes expire, how long it takes for them to shrink and disappear."));
    }

    private static void ResetRendering()
    {
        MappingCentralNodeSize.Value = (float)MappingCentralNodeSize.DefaultValue;
        MappingUseDirectionalNodes.Value = (bool)MappingUseDirectionalNodes.DefaultValue;
        MappingSpecialDirectionalNodeColors.Value = (bool)MappingSpecialDirectionalNodeColors.DefaultValue;
        MappingDirectionalNodeLength.Value = (float)MappingDirectionalNodeLength.DefaultValue;
        MappingDirectionalNodeDiameter.Value = (float)MappingDirectionalNodeDiameter.DefaultValue;
        MappingDirectionalNodeOffset.Value = (float)MappingDirectionalNodeOffset.DefaultValue;
        MappingNodeColorNormal.Value = (string)MappingNodeColorNormal.DefaultValue;
        MappingNodeColorVent.Value = (string)MappingNodeColorVent.DefaultValue;
        MappingNodeColorPeek.Value = (string)MappingNodeColorPeek.DefaultValue;
        MappingNodeSpawnTime.Value = (float)MappingNodeSpawnTime.DefaultValue;
        MappingNodeDespawnTime.Value = (float)MappingNodeDespawnTime.DefaultValue;
    }
}