using System.Collections.Generic;
using UnityEngine;
using BepInEx.Logging;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Renderers;

public static class VentRendererPool
{
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
}