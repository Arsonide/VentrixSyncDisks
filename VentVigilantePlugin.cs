using Il2CppInterop.Runtime.Injection;
using BepInEx;
using SOD.Common.BepInEx;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Config;
using VentVigilante.Implementation.Renderers;

namespace VentVigilante;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class VentVigilantePlugin : PluginController<VentVigilantePlugin>
{
    public override void Load()
    {
        base.Load();
        
        VentVigilanteConfig.Initialize(Config);
        
        if (!VentVigilanteConfig.Enabled.Value)
        {
            Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is disabled.");
            return;
        }

        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.PatchAll();
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        ClassInjector.RegisterTypeInIl2Cpp<VentRenderer>();       
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");
        
        Initialize();
    }

    public override bool Unload()
    {
        Uninitialize();
        return base.Unload();
    }

    private void Initialize()
    {
        SyncDisks.Initialize();
    }
    
    private void Uninitialize()
    {
        VentRendererPool.CleanupVentRenderers();
        SyncDisks.Uninitialize();
    }
}