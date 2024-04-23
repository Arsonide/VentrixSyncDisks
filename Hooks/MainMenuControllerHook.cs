using BepInEx.Unity.IL2CPP.UnityEngine;
using HarmonyLib;
using UnityEngine;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Renderers;
using KeyCode = BepInEx.Unity.IL2CPP.UnityEngine.KeyCode;

namespace VentVigilante.Hooks;

[HarmonyPatch(typeof(InteractableController), "OnCollisionEnter")]
public class MainMenuControllerHook
{
    [HarmonyPostfix]
    public static void Postfix(InteractableController __instance)
    {
        if (__instance.thrownBy != Player.Instance)
        {
            return;
        }

        if (__instance.rb.velocity.magnitude < 0.1f)
        {
            return;
        }
        
        if (!__instance.gameObject.name.StartsWith("WorldCoin"))
        {
            return;
        }

        Vector3 coinPosition = __instance.transform.position;

        foreach (AirDuctGroup.AirDuctSection airDuct in VentHelpers.PositionToAirDucts(coinPosition, 5000f))
        {
            var tempDuct = airDuct;

            int safety = 100;
            
            while (tempDuct.node.building.ductMap.TryGetValue(tempDuct.next, out AirDuctGroup.AirDuctSection newSection) && safety > 0)
            {
                tempDuct = newSection;
                safety--;
                
                if (VentRendererPool.SeedRenderer == null)
                {
                    VentRendererPool.CreateSeedRenderer();
                }
            
                Vector3 position = VentHelpers.AirDuctToPosition(tempDuct);
                GameObject go = Object.Instantiate(VentRendererPool.SeedRenderer);
                go.SetActive(true);
                go.transform.SetPositionAndRotation(position, Quaternion.identity);
            }
            
            tempDuct = airDuct;
            safety = 100;
            
            while (tempDuct.node.building.ductMap.TryGetValue(tempDuct.previous, out AirDuctGroup.AirDuctSection newSection) && safety > 0)
            {
                tempDuct = newSection;
                safety--;
                
                if (VentRendererPool.SeedRenderer == null)
                {
                    VentRendererPool.CreateSeedRenderer();
                }
            
                Vector3 position = VentHelpers.AirDuctToPosition(tempDuct);
                GameObject go = Object.Instantiate(VentRendererPool.SeedRenderer);
                go.SetActive(true);
                go.transform.SetPositionAndRotation(position, Quaternion.identity);
            }
        }
        
        return;
        if (Input.GetKeyInt(KeyCode.Keypad5))
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            // Create a new material with HDRP/Lit shader
            Material invisibleMaterial = new Material(Shader.Find("HDRP/Lit"))
            {
                renderQueue = 0,
            };

            // Optionally apply this material to a GameObject
            // For example, apply to the current GameObject if needed
            if (cube.GetComponent<Renderer>())
            {
                cube.GetComponent<Renderer>().material = invisibleMaterial;
            }
            
            cube.layer = 30;

            if (Camera.main != null)
            {
                Transform t = Camera.main.transform;
                cube.transform.position = t.position;
                cube.transform.rotation = t.rotation;
            }

            cube.transform.localScale = new Vector3(1, 1, 1);
            cube.GetComponent<Renderer>().material.color = Color.magenta;
        }
        
        //return true;
    }
}