using UnityEngine;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class DuctMarker : BasePoolObject
{
    private static readonly Vector3 _minimumScale = new Vector3(float.Epsilon, float.Epsilon, float.Epsilon);
    
    public Renderer Renderer;
    public Transform Transform;
    
    private Vector3 _targetScale;
    private float _lifetime;
    private float _timer;
    private DuctMarkerState _state;

    public void OnSpawn(Vector3 targetScale, float lifetime)
    {
        _targetScale = targetScale;
        _lifetime = lifetime;
        SetState(DuctMarkerState.Spawning, true);
        Transform.localScale = _minimumScale;
    }
    
    private void OnDespawn()
    {
        DuctMarkerPool.Instance.ReleaseMarker(this);
    }

    private void SetState(DuctMarkerState newState, bool force = false)
    {
        DuctMarkerState oldState = _state;

        if (!force && newState == oldState)
        {
            return;
        }

        _state = newState;
        _timer = 0f;

        switch (_state)
        {
            case DuctMarkerState.Spawning:
                Transform.localScale = _minimumScale;
                break;
            case DuctMarkerState.Spawned:
                Transform.localScale = _targetScale;
                break;
            case DuctMarkerState.Despawning:
                Transform.localScale = _targetScale;
                break;
            case DuctMarkerState.Despawned:
                Transform.localScale = _minimumScale;
                OnDespawn();
                break;
        }
    }
    
    private void Update()
    {
        _timer += GetAdjustedDeltaTime();

        switch (_state)
        {
            case DuctMarkerState.Spawning:
                UpdateSpawning();
                break;
            case DuctMarkerState.Spawned:
                UpdateSpawned();
                break;
            case DuctMarkerState.Despawning:
                UpdateDespawning();
                break;
        }
    }

    private void UpdateSpawning()
    {
        Transform.localScale = Vector3.Lerp(_minimumScale, _targetScale, _timer / VentrixConfig.RenderingNodeSpawnTime.Value);
        
        if (_timer >= VentrixConfig.RenderingNodeSpawnTime.Value)
        {
            SetState(DuctMarkerState.Spawned);
        }
    }
    
    private void UpdateSpawned()
    {
        if (_timer >= _lifetime)
        {
            SetState(DuctMarkerState.Despawning);
        }
    }
    
    private void UpdateDespawning()
    {
        Transform.localScale = Vector3.Lerp(_targetScale, _minimumScale, _timer / VentrixConfig.RenderingNodeDespawnTime.Value);
        
        if (_timer >= VentrixConfig.RenderingNodeDespawnTime.Value)
        {
            SetState(DuctMarkerState.Despawned);
        }
    }
    
    private float GetAdjustedDeltaTime()
    {
        float dt = Time.deltaTime;

        if (_state != DuctMarkerState.Spawned)
        {
            return dt;
        }
        
        int level = DiskRegistry.MappingDisk.Level;

        if (level <= 0)
        {
            return dt;
        }

        FirstPersonItem firstPersonItem = FirstPersonItemController.Instance.currentItem;

        if (firstPersonItem != null && firstPersonItem.presetName.StartsWith("coin"))
        {
            return dt * VentrixConfig.MappingCoinMultiplier.GetLevel(level);
        }

        return dt;
    }
}