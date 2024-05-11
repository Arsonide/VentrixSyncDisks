using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class DuctMarkerPool : BasePoolManager<DuctMarker>
{
    public static DuctMarkerPool Instance;

    private static Material _normalDuctMaterial;
    private static Material _peekDuctMaterial;
    private static Material _normalVentMaterial;

    protected override void SetupManager()
    {
        base.SetupManager();
        
        Shader unlitShader = Shader.Find("HDRP/Unlit");
        int unlitColor = Shader.PropertyToID("_UnlitColor");
        int zTestDepthEqualForOpaque = Shader.PropertyToID("_ZTestDepthEqualForOpaque");
        int zWrite = Shader.PropertyToID("_ZWrite");
         
        _normalDuctMaterial = new Material(unlitShader);
        _normalDuctMaterial.SetColor(unlitColor, Utilities.HexToColor(VentrixConfig.RenderingNodeColorNormal.Value));
        _normalDuctMaterial.SetInt(zTestDepthEqualForOpaque, (int)UnityEngine.Rendering.CompareFunction.Always);
        _normalDuctMaterial.SetInt(zWrite, 0);

        _peekDuctMaterial = new Material(unlitShader);
        _peekDuctMaterial.SetColor(unlitColor, Utilities.HexToColor(VentrixConfig.RenderingNodeColorPeek.Value));
        _peekDuctMaterial.SetInt(zTestDepthEqualForOpaque, (int)UnityEngine.Rendering.CompareFunction.Always);
        _peekDuctMaterial.SetInt(zWrite, 0);
        
        _normalVentMaterial = new Material(unlitShader);
        _normalVentMaterial.SetColor(unlitColor, Utilities.HexToColor(VentrixConfig.RenderingNodeColorVent.Value));
        _normalVentMaterial.SetInt(zTestDepthEqualForOpaque, (int)UnityEngine.Rendering.CompareFunction.Always);
        _normalVentMaterial.SetInt(zWrite, 0);
    }

    protected override DuctMarker CreateBaseObject()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer r = cube.GetComponent<Renderer>();
        r.sharedMaterial = _normalDuctMaterial;

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
                ventRenderer.Renderer.sharedMaterial = _normalDuctMaterial;
                break;
            case DuctMarkerType.PeekDuct:
                ventRenderer.Renderer.sharedMaterial = _peekDuctMaterial;
                break;
            case DuctMarkerType.NormalVent:
                ventRenderer.Renderer.sharedMaterial = _normalVentMaterial;
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