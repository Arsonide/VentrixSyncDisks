using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

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