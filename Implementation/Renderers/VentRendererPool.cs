using System.Collections.Generic;
using UnityEngine;
using BepInEx.Logging;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Renderers;

public static class VentRendererPool
{
    public static GameObject SeedRenderer = null;
    
    private static List<VentRenderer> AvailableRenderers = new List<VentRenderer>();
    private static List<VentRenderer> AllRenderers = new List<VentRenderer>();
    
    public static void Create(Vector3 position, float time)
    {
        VentRenderer ventRenderer = GetVentRenderer();
        ventRenderer.transform.SetPositionAndRotation(position, Quaternion.identity);
        ventRenderer.DespawnTimer = time;
    }
    
    private static VentRenderer GetVentRenderer()
    {
        VentRenderer ventRenderer;
        int lastIndex = AvailableRenderers.Count - 1;

        if (lastIndex >= 0)
        {
            ventRenderer = AvailableRenderers[lastIndex];
            AvailableRenderers.RemoveAt(lastIndex);
            ventRenderer.gameObject.SetActive(true);
        }
        else
        {
            GameObject go = new GameObject("VentRenderer");
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            ventRenderer = go.AddComponent<VentRenderer>();
            Object.DontDestroyOnLoad(go);
            AllRenderers.Add(ventRenderer);
            Utilities.Log($"Created vent renderer, current count is {AllRenderers.Count}.", LogLevel.Debug);
        }
        
        return ventRenderer;
    }

    public static void ReleaseVentRenderer(VentRenderer ventRenderer)
    {
        ventRenderer.gameObject.SetActive(false);
        AvailableRenderers.Add(ventRenderer);
    }

    public static void CleanupVentRenderers()
    {
        for (int i = AllRenderers.Count - 1; i >= 0; --i)
        {
            VentRenderer ventRenderer = AllRenderers[i];

            if (ventRenderer.gameObject.activeSelf)
            {
                ventRenderer.gameObject.SetActive(false);
            }

            Object.DestroyImmediate(ventRenderer.gameObject);
        }
        
        Utilities.Log("Cleaned up all vent renderers.", LogLevel.Debug);
    }

    public static void CreateSeedRenderer()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        Transform t = cube.transform;
        Renderer r = cube.GetComponent<Renderer>();
        Collider c = cube.GetComponent<Collider>();
        BoxCollider bc = cube.GetComponent<BoxCollider>();

        // This is technically an opaque material but it's behind everything so it's "invisible".
        // For the outline effect to work it must be visible, for some reason.
        Material m = new Material(Shader.Find("HDRP/Unlit"));
        m.SetColor("_UnlitColor", Color.yellow);
        m.SetInt("_ZTestDepthEqualForOpaque", (int)UnityEngine.Rendering.CompareFunction.Always);
        m.SetInt("_ZWrite", 0);
        //m.renderQueue = 5000;

        if (r != null)
        {
            r.sharedMaterial = m;
        }

        Debug.Log("ghghkkgjhG" + r.sharedMaterial.renderQueue);

        if (c != null)
        {
            Debug.Log("FOUND COLLIDER 1, DESTROYING");
            Object.Destroy(c);
        }
        
        if (bc != null)
        {
            Debug.Log("FOUND COLLIDER 2, DESTROYING");
            Object.Destroy(bc);
        }
        
        // This is the outline layer, it's a post processing effect that picks up anything on layer 30. Yep.
        //cube.layer = 30;

        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        Object.DontDestroyOnLoad(cube);
        cube.SetActive(false);

        SeedRenderer = cube;
        
        Component[] components = cube.GetComponents<Component>();

        Debug.Log("Listing all components on: " + cube.name);

        // Iterate through all components and print their type
        foreach (Component component in components)
        {
            Debug.Log(component.name);
            Debug.Log(component.ToString());
            Debug.Log(component.GetType().ToString());
            Debug.Log(component.GetType().Name);
            Debug.Log(component.GetType().FullName);
            Debug.Log(component.GetType().AssemblyQualifiedName);
        }
    }
}