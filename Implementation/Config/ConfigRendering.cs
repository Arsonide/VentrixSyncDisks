using BepInEx.Configuration;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    private const string NAME_SHORT_RENDERING = "Rendering";
    private const int ID_RENDERING = 8;

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
        string section = $"{ID_RENDERING}. {NAME_SHORT_RENDERING}";

        _renderingCentralNodeSize = config.Bind(section, "Central Node Size", 0.1f,
                                             new ConfigDescription("When rendering vent networks, how large the visualized nodes are in the center of each duct."));

        _renderingUseDirectionalNodes = config.Bind(section, "Use Directional Nodes", true,
                                                 new ConfigDescription("When rendering vent networks, whether to render additional indicators pointing at connected air ducts."));

        _renderingSpecialDirectionalNodeColors = config.Bind(section, "Special Directional Node Colors", false,
                                                 new ConfigDescription("When rendering vent networks, whether additional connections on vents are colored as normal ducts or as vents."));
        
        _renderingDirectionalNodeLength = config.Bind(section, "Directional Node Length", 0.5f,
                                                   new ConfigDescription("When rendering vent networks, how long the indicators pointing down connected ducts are."));

        _renderingDirectionalNodeDiameter = config.Bind(section, "Directional Node Diameter", 0.015f,
                                                     new ConfigDescription("When rendering vent networks, how wide the indicators pointing down connected ducts are."));

        _renderingDirectionalNodeOffset = config.Bind(section, "Directional Node Offset", 0.525f,
                                                   new ConfigDescription("When rendering vent networks, how far apart the indicators pointing down connected ducts are."));

        _renderingNodeColorNormal = config.Bind(section, "Node Color Normal", "00FFFF",
                                             new ConfigDescription("When rendering vent networks, what color normal air ducts are visualized as."));

        _renderingNodeColorVent = config.Bind(section, "Node Color Vent", "FFFF00",
                                             new ConfigDescription("When rendering vent networks, what color air vents (entrances / exits) are visualized as."));

        _renderingNodeColorPeek = config.Bind(section, "Node Color Peek", "00FFFF",
                                             new ConfigDescription("When rendering vent networks, what color \"peek\" ducts are visualized as, that you can see through but not exit."));
        
        _renderingNodeSpawnTime = config.Bind(section, "Node Spawn Time", 0.2f,
                                                   new ConfigDescription("When rendering vent networks, when nodes spawn in, how long it takes for them to reach full size."));
        
        _renderingNodeDespawnTime = config.Bind(section, "Node Despawn Time", 0.2f,
                                             new ConfigDescription("When rendering vent networks, when nodes expire, how long it takes for them to shrink and disappear."));
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