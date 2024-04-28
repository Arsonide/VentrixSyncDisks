using UnityEngine;
using VentVigilante.Implementation.Pooling;

namespace VentVigilante.Implementation.Mapping;

public class DuctMarkerPool : BasePoolManager<DuctMarker>
{
    public static DuctMarkerPool Instance;
    
    private static Material NormalDuctMaterial;
    private static Material PeekDuctMaterial;
    private static Material NormalVentMaterial;

    protected override void SetupManager()
    {
        base.SetupManager();
        
        NormalDuctMaterial = new Material(Shader.Find("HDRP/Unlit"));
        NormalDuctMaterial.SetColor("_UnlitColor", Color.blue);
        NormalDuctMaterial.SetInt("_ZTestDepthEqualForOpaque", (int)UnityEngine.Rendering.CompareFunction.Always);
        NormalDuctMaterial.SetInt("_ZWrite", 0);

        PeekDuctMaterial = new Material(NormalDuctMaterial);
        PeekDuctMaterial.SetColor("_UnlitColor", Color.yellow);
        
        NormalVentMaterial = new Material(NormalDuctMaterial);
        NormalVentMaterial.SetColor("_UnlitColor", Color.green);
    }

    protected override DuctMarker CreateBaseObject()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer r = cube.GetComponent<Renderer>();
        r.sharedMaterial = NormalDuctMaterial;

        DuctMarker ductMarker = cube.AddComponent<DuctMarker>();
        ductMarker.Renderer = r;
        
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
    }

    public void CreateMarker(DuctMarkerType type, Vector3 position, float time)
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
        
        ventRenderer.transform.SetPositionAndRotation(position, Quaternion.identity);
        ventRenderer.DespawnTimer = time;
    }

    public void ReleaseMarker(DuctMarker marker)
    {
        CheckinPoolObject(marker);
    }
}