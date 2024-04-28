using UnityEngine;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Disks;
using VentVigilante.Implementation.Pooling;

namespace VentVigilante.Implementation.Mapping;

public class DuctMarker : BasePoolObject
{
    public Renderer Renderer;
    public float DespawnTimer;

    private void Update()
    {
        float dt = Time.deltaTime;

        if (DiskRegistry.MappingDisk.Level >= 3)
        {
            FirstPersonItem firstPersonItem = FirstPersonItemController.Instance.currentItem;

            if (firstPersonItem != null && firstPersonItem.presetName.StartsWith("coin"))
            {
                dt *= 0.1f;
            }
        }
        
        DespawnTimer -= dt;

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