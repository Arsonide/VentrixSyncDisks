using UnityEngine;

namespace VentVigilante.Implementation.Renderers;

public class DuctMapCube : MonoBehaviour
{
    public Renderer Renderer;
    public float DespawnTimer;

    public void Initialize()
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
        DuctMapCubePool.ReleaseVentRenderer(this);
    }
}