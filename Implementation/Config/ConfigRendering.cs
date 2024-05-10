using BepInEx.Configuration;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private const int ID_RENDERING = 4;

    public static ConfigCacheSimple<float> RenderingCentralNodeSize;
    public static ConfigCacheSimple<bool> RenderingUseDirectionalNodes;
    public static ConfigCacheSimple<bool> RenderingSpecialDirectionalNodeColors;
    public static ConfigCacheSimple<float> RenderingDirectionalNodeLength;
    public static ConfigCacheSimple<float> RenderingDirectionalNodeDiameter;
    public static ConfigCacheSimple<float> RenderingDirectionalNodeOffset;

    public static ConfigCacheSimple<string> RenderingNodeColorNormal;
    public static ConfigCacheSimple<string> RenderingNodeColorVent;
    public static ConfigCacheSimple<string> RenderingNodeColorPeek;

    public static ConfigCacheSimple<float> RenderingNodeSpawnTime;
    public static ConfigCacheSimple<float> RenderingNodeDespawnTime;

    private static ConfigEntry<float> _renderingCentralNodeSize;
    private static ConfigEntry<bool> _renderingUseDirectionalNodes;
    private static ConfigEntry<bool> _renderingSpecialDirectionalNodeColors;
    private static ConfigEntry<float> _renderingDirectionalNodeLength;
    private static ConfigEntry<float> _renderingDirectionalNodeDiameter;
    private static ConfigEntry<float> _renderingDirectionalNodeOffset;

    private static ConfigEntry<string> _renderingNodeColorNormal;
    private static ConfigEntry<string> _renderingNodeColorVent;
    private static ConfigEntry<string> _renderingNodeColorPeek;

    private static ConfigEntry<float> _renderingNodeSpawnTime;
    private static ConfigEntry<float> _renderingNodeDespawnTime;

    private static void InitializeRendering(ConfigFile config)
    {
        _renderingCentralNodeSize = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Central Node Size", 0.1f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how large the visualized nodes are in the center of each duct."));

        _renderingUseDirectionalNodes = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Use Directional Nodes", true,
                                                 new ConfigDescription($"When using  {NAME_LONG_MAPPING}, whether to render additional indicators pointing at connected air ducts."));

        _renderingSpecialDirectionalNodeColors = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Special Directional Node Colors", false,
                                                 new ConfigDescription($"When using  {NAME_LONG_MAPPING}, whether additional connections on vents are colored as normal ducts or as vents."));
        
        _renderingDirectionalNodeLength = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Length", 0.5f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how long the indicators pointing down connected ducts are."));

        _renderingDirectionalNodeDiameter = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Diameter", 0.015f,
                                                     new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how wide the indicators pointing down connected ducts are."));

        _renderingDirectionalNodeOffset = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Directional Node Offset", 0.525f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, how far apart the indicators pointing down connected ducts are."));

        _renderingNodeColorNormal = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Normal", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color normal air ducts are visualized as."));

        _renderingNodeColorVent = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Vent", "FFFF00",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color air vents (entrances / exits) are visualized as."));

        _renderingNodeColorPeek = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Color Peek", "00FFFF",
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, what color \"peek\" ducts are visualized as, that you can see through but not exit."));
        
        _renderingNodeSpawnTime = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Spawn Time", 0.2f,
                                                   new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes spawn in, how long it takes for them to reach full size."));
        
        _renderingNodeDespawnTime = config.Bind($"{ID_RENDERING}. {NAME_SHORT_MAPPING} Rendering", "Node Despawn Time", 0.2f,
                                             new ConfigDescription($"When using  {NAME_LONG_MAPPING}, when nodes expire, how long it takes for them to shrink and disappear."));
    }

    private static void CacheRendering()
    {
        RenderingCentralNodeSize = new ConfigCacheSimple<float>(_renderingCentralNodeSize);
        RenderingUseDirectionalNodes = new ConfigCacheSimple<bool>(_renderingUseDirectionalNodes);
        RenderingSpecialDirectionalNodeColors = new ConfigCacheSimple<bool>(_renderingSpecialDirectionalNodeColors);
        RenderingDirectionalNodeLength = new ConfigCacheSimple<float>(_renderingDirectionalNodeLength);
        RenderingDirectionalNodeDiameter = new ConfigCacheSimple<float>(_renderingDirectionalNodeDiameter);
        RenderingDirectionalNodeOffset = new ConfigCacheSimple<float>(_renderingDirectionalNodeOffset);
        RenderingNodeColorNormal = new ConfigCacheSimple<string>(_renderingNodeColorNormal);
        RenderingNodeColorVent = new ConfigCacheSimple<string>(_renderingNodeColorVent);
        RenderingNodeColorPeek = new ConfigCacheSimple<string>(_renderingNodeColorPeek);
        RenderingNodeSpawnTime = new ConfigCacheSimple<float>(_renderingNodeSpawnTime);
        RenderingNodeDespawnTime = new ConfigCacheSimple<float>(_renderingNodeDespawnTime);
    }
    
    private static void ResetRendering()
    {
        _renderingCentralNodeSize.Value = (float)_renderingCentralNodeSize.DefaultValue;
        _renderingUseDirectionalNodes.Value = (bool)_renderingUseDirectionalNodes.DefaultValue;
        _renderingSpecialDirectionalNodeColors.Value = (bool)_renderingSpecialDirectionalNodeColors.DefaultValue;
        _renderingDirectionalNodeLength.Value = (float)_renderingDirectionalNodeLength.DefaultValue;
        _renderingDirectionalNodeDiameter.Value = (float)_renderingDirectionalNodeDiameter.DefaultValue;
        _renderingDirectionalNodeOffset.Value = (float)_renderingDirectionalNodeOffset.DefaultValue;
        _renderingNodeColorNormal.Value = (string)_renderingNodeColorNormal.DefaultValue;
        _renderingNodeColorVent.Value = (string)_renderingNodeColorVent.DefaultValue;
        _renderingNodeColorPeek.Value = (string)_renderingNodeColorPeek.DefaultValue;
        _renderingNodeSpawnTime.Value = (float)_renderingNodeSpawnTime.DefaultValue;
        _renderingNodeDespawnTime.Value = (float)_renderingNodeDespawnTime.DefaultValue;
    }
}