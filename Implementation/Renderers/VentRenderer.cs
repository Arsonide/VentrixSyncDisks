using UnityEngine;

namespace VentVigilante.Implementation.Renderers;

public class VentRenderer : MonoBehaviour
{
    public float DespawnTimer;
    private Renderer renderer;
    
    private void Awake()
    {
        
    }

    private void OnDestroy()
    {

    }

    private void Update()
    {
        DespawnTimer -= Time.deltaTime;

        if (DespawnTimer <= 0f)
        {
            OnDespawn();
        }
    }
    
    private void OnDespawn()
    {
        VentRendererPool.ReleaseVentRenderer(this);
    }
}