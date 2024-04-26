using System.Collections.Generic;
using UnityEngine;
using BepInEx.Logging;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Renderers;

public static class DuctMapCubePool
{
    public static DuctMapCube BaseDuctMapCube = null;
    
    private static List<DuctMapCube> AvailableRenderers = new List<DuctMapCube>();
    private static List<DuctMapCube> AllRenderers = new List<DuctMapCube>();

    private static Material NormalDuctMaterial;
    private static Material PeekDuctMaterial;
    private static Material NormalVentMaterial;
    
    public static void Initialize()
    {
        NormalDuctMaterial = new Material(Shader.Find("HDRP/Unlit"));
        NormalDuctMaterial.SetColor("_UnlitColor", Color.blue);
        NormalDuctMaterial.SetInt("_ZTestDepthEqualForOpaque", (int)UnityEngine.Rendering.CompareFunction.Always);
        NormalDuctMaterial.SetInt("_ZWrite", 0);

        PeekDuctMaterial = new Material(NormalDuctMaterial);
        PeekDuctMaterial.SetColor("_UnlitColor", Color.yellow);
        
        NormalVentMaterial = new Material(NormalDuctMaterial);
        NormalVentMaterial.SetColor("_UnlitColor", Color.green);
        
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Renderer r = cube.GetComponent<Renderer>();
        r.sharedMaterial = NormalDuctMaterial;

        DuctMapCube ductMapCube = cube.AddComponent<DuctMapCube>();
        ductMapCube.Renderer = r;
        
        Collider c = cube.GetComponent<Collider>();
        Object.Destroy(c);

        Transform t = cube.transform;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        t.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Object.DontDestroyOnLoad(cube);
        cube.SetActive(false);

        BaseDuctMapCube = ductMapCube;
    }
    
    public static void Create(DuctMapCubeType type, Vector3 position, float time)
    {
        DuctMapCube ventRenderer = GetDuctMapCube();

        if (ventRenderer == null)
        {
            Utilities.Log("Pulled a null renderer out of the pool.");
            return;
        }

        if (ventRenderer.Renderer == null)
        {
            Utilities.Log("Renderer was null");
            return;
        }
        
        switch (type)
        {
            case DuctMapCubeType.NormalDuct:
                ventRenderer.Renderer.sharedMaterial = NormalDuctMaterial;
                break;
            case DuctMapCubeType.PeekDuct:
                ventRenderer.Renderer.sharedMaterial = PeekDuctMaterial;
                break;
            case DuctMapCubeType.NormalVent:
                ventRenderer.Renderer.sharedMaterial = NormalVentMaterial;
                break;
        }
        
        ventRenderer.transform.SetPositionAndRotation(position, Quaternion.identity);
        ventRenderer.DespawnTimer = time;
    }
    
    private static DuctMapCube GetDuctMapCube()
    {
        DuctMapCube ductMapCube;
        int lastIndex = AvailableRenderers.Count - 1;

        if (lastIndex >= 0)
        {
            ductMapCube = AvailableRenderers[lastIndex];
            AvailableRenderers.RemoveAt(lastIndex);
            ductMapCube.gameObject.SetActive(true);
        }
        else
        {
            ductMapCube = Object.Instantiate(BaseDuctMapCube);
            Object.DontDestroyOnLoad(ductMapCube.gameObject);
            ductMapCube.gameObject.SetActive(true);
            ductMapCube.Renderer = ductMapCube.gameObject.GetComponent<Renderer>();
            AllRenderers.Add(ductMapCube);
            Utilities.Log($"Created vent renderer, current count is {AllRenderers.Count}.", LogLevel.Debug);
        }
        
        return ductMapCube;
    }

    public static void ReleaseVentRenderer(DuctMapCube ductMapCube)
    {
        ductMapCube.gameObject.SetActive(false);
        AvailableRenderers.Add(ductMapCube);
    }

    public static void CleanupVentRenderers()
    {
        for (int i = AllRenderers.Count - 1; i >= 0; --i)
        {
            DuctMapCube ductMapCube = AllRenderers[i];

            if (ductMapCube.gameObject.activeSelf)
            {
                ductMapCube.gameObject.SetActive(false);
            }

            Object.DestroyImmediate(ductMapCube.gameObject);
        }
        
        Utilities.Log("Cleaned up all vent renderers.", LogLevel.Debug);
    }
}