using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
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
        int level = DiskRegistry.MappingDisk.Level;
        
        if (level > 0)
        {
            FirstPersonItem firstPersonItem = FirstPersonItemController.Instance.currentItem;

            if (firstPersonItem != null && firstPersonItem.presetName.StartsWith("coin"))
            {
                dt *= VentrixConfig.MappingCoinMultiplier.GetLevel(level);
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