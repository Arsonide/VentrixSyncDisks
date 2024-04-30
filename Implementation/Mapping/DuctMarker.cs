using UnityEngine;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class DuctMarker : BasePoolObject
{
    private static readonly Vector3 MINIMUM_SCALE = new Vector3(float.Epsilon, float.Epsilon, float.Epsilon);
    
    public Renderer Renderer;
    public Transform Transform;
    
    private Vector3 _targetScale;
    private float _lifetime;
    private float _timer;
    private DuctMarkerState _state;
    
    private const float SPAWN_DURATION = 0.2f;
    private const float DESPAWN_DURATION = 0.2f;

    public void OnSpawn(Vector3 targetScale, float lifetime)
    {
        _targetScale = targetScale;
        _lifetime = lifetime;
        SetState(DuctMarkerState.Spawning, true);
        Transform.localScale = MINIMUM_SCALE;
    }
    
    private void OnDespawn()
    {
        DuctMarkerPool.Instance.ReleaseMarker(this);
    }

    private void SetState(DuctMarkerState newState, bool force = true)
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
                Transform.localScale = MINIMUM_SCALE;
                break;
            case DuctMarkerState.Spawned:
                Transform.localScale = _targetScale;
                break;
            case DuctMarkerState.Despawning:
                Transform.localScale = _targetScale;
                break;
            case DuctMarkerState.Despawned:
                Transform.localScale = MINIMUM_SCALE;
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
        Transform.localScale = Vector3.Lerp(MINIMUM_SCALE, _targetScale, _timer / SPAWN_DURATION);
        
        if (_timer >= SPAWN_DURATION)
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
        Transform.localScale = Vector3.Lerp(_targetScale, MINIMUM_SCALE, _timer / DESPAWN_DURATION);
        
        if (_timer >= DESPAWN_DURATION)
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