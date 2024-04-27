using UnityEngine;
using VentVigilante.Implementation.Pooling;

namespace VentVigilante.Implementation.Markers;

public class DuctMarker : BasePoolObject
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
        DuctMarkerPool.Instance.ReleaseMarker(this);
    }
}