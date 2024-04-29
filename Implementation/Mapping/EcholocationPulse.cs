using System.Collections;
using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class EcholocationPulse : BasePoolObject
{
    private Coroutine _pulseCoroutine;
    private DuctExplorer _explorer = new DuctExplorer();

    public override void OnCheckout()
    {
        base.OnCheckout();
        StopPulse();
        _explorer.Reset();
    }

    public override void OnCheckin()
    {
        base.OnCheckin();
        StopPulse();
        _explorer.Reset();
    }

    public void StopPulse()
    {
        if (_pulseCoroutine != null)
        {
            UniverseLib.RuntimeHelper.StopCoroutine(_pulseCoroutine);
            _pulseCoroutine = null;
        }
    }

    public void StartPulse(AirDuctGroup.AirDuctSection startDuct)
    {
        StopPulse();
        _pulseCoroutine = UniverseLib.RuntimeHelper.StartCoroutine(PulseRoutine(startDuct));
    }
    
    [HideFromIl2Cpp]
    private IEnumerator PulseRoutine(AirDuctGroup.AirDuctSection startDuct)
    {
        _explorer.StartExploration(startDuct);

        int range = 25;

        if (DiskRegistry.MappingDisk.Level >= 2)
        {
            range = 50;
        }
        
        for (int i = 0; i < range; ++i)
        {
            List<DuctExplorerTick> results = _explorer.TickExploration(startDuct);

            foreach (DuctExplorerTick result in results)
            {
                Vector3 position = VentHelpers.AirDuctToPosition(result.Duct);
                DuctMarkerPool.Instance.CreateMarker(result.Type, position, 1f);
            }

            yield return EcholocationPulsePool.Instance.PulseDelay;
        }

        EcholocationPulsePool.Instance.CheckinPoolObject(this);
    }
}