using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class DuctMarkerPool : BasePoolManager<DuctMarker>
{
    public static DuctMarkerPool Instance;
    public static float MarkerSpawnTime = 0.2f;
    public static float MarkerDespawnTime = 0.2f;
    
    private static Material NormalDuctMaterial;
    private static Material PeekDuctMaterial;
    private static Material NormalVentMaterial;

    protected override void SetupManager()
    {
        base.SetupManager();
        
        NormalDuctMaterial = new Material(Shader.Find("HDRP/Unlit"));
        NormalDuctMaterial.SetColor("_UnlitColor", Utilities.HexToColor(VentrixConfig.MappingNodeColorNormal.Value));
        NormalDuctMaterial.SetInt("_ZTestDepthEqualForOpaque", (int)UnityEngine.Rendering.CompareFunction.Always);
        NormalDuctMaterial.SetInt("_ZWrite", 0);

        PeekDuctMaterial = new Material(NormalDuctMaterial);
        PeekDuctMaterial.SetColor("_UnlitColor", Utilities.HexToColor(VentrixConfig.MappingNodeColorPeek.Value));
        
        NormalVentMaterial = new Material(NormalDuctMaterial);
        NormalVentMaterial.SetColor("_UnlitColor", Utilities.HexToColor(VentrixConfig.MappingNodeColorVent.Value));

        MarkerSpawnTime = VentrixConfig.MappingNodeSpawnTime.Value;
        MarkerDespawnTime = VentrixConfig.MappingNodeDespawnTime.Value;
    }

    protected override DuctMarker CreateBaseObject()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer r = cube.GetComponent<Renderer>();
        r.sharedMaterial = NormalDuctMaterial;

        DuctMarker ductMarker = cube.AddComponent<DuctMarker>();
        ductMarker.Renderer = r;
        ductMarker.Transform = ductMarker.transform;
        
        Collider c = cube.GetComponent<Collider>();
        Object.Destroy(c);

        Transform t = cube.transform;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        t.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Object.DontDestroyOnLoad(cube);
        cube.SetActive(false);

        return ductMarker;
    }

    protected override void ConfigureCopyObject(ref DuctMarker copyObject)
    {
        copyObject.Renderer = copyObject.gameObject.GetComponent<Renderer>();
        copyObject.Transform = copyObject.transform;
    }

    public void CreateMarker(DuctMarkerType type, Vector3 position, Vector3 scale, float time)
    {
        DuctMarker ventRenderer = CheckoutPoolObject();

        switch (type)
        {
            case DuctMarkerType.NormalDuct:
                ventRenderer.Renderer.sharedMaterial = NormalDuctMaterial;
                break;
            case DuctMarkerType.PeekDuct:
                ventRenderer.Renderer.sharedMaterial = PeekDuctMaterial;
                break;
            case DuctMarkerType.NormalVent:
                ventRenderer.Renderer.sharedMaterial = NormalVentMaterial;
                break;
        }

        Transform t = ventRenderer.transform;
        t.SetPositionAndRotation(position, Quaternion.identity);
        t.localScale = scale;
        
        ventRenderer.OnSpawn(scale, time);
    }

    public void ReleaseMarker(DuctMarker marker)
    {
        CheckinPoolObject(marker);
    }
}