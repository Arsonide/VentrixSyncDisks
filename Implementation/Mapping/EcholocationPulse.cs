using System.Collections;
using System.Collections.Generic;
using Il2CppInterop.Runtime.Attributes;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Mapping.Explorer;
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
        if (_pulseCoroutine == null)
        {
            return;
        }

        UniverseLib.RuntimeHelper.StopCoroutine(_pulseCoroutine);
        _pulseCoroutine = null;
    }

    public void StartPulse(AirDuctGroup.AirDuctSection startDuct)
    {
        StopPulse();
        _pulseCoroutine = UniverseLib.RuntimeHelper.StartCoroutine(PulseRoutine(startDuct));
    }
    
    [HideFromIl2Cpp]
    private IEnumerator PulseRoutine(AirDuctGroup.AirDuctSection startDuct)
    {
        int level = DiskRegistry.MappingDisk.Level;

        if (level <= 0)
        {
            yield break;
        }
        
        _explorer.StartExploration(startDuct);

        int pulseRange = VentrixConfig.MappingEcholocationRange.GetLevel(level);
        float pulseDuration = VentrixConfig.MappingEcholocationDuration.GetLevel(level);

        for (int i = 0; i < pulseRange; ++i)
        {
            List<DuctExplorerTick> results = _explorer.TickExploration(startDuct);

            foreach (DuctExplorerTick result in results)
            {
                Vector3 position = VentHelpers.AirDuctToPosition(result.Duct);
                SpawnNodes(result, position, pulseDuration);
            }

            yield return EcholocationPulsePool.Instance.PulseDelays[level];
        }

        EcholocationPulsePool.Instance.CheckinPoolObject(this);
    }

    private void SpawnNodes(DuctExplorerTick result, Vector3 position, float pulseDuration)
    {
        DuctMarkerPool.Instance.CreateMarker(result.Type, position, new Vector3(VentrixConfig.RenderingCentralNodeSize.Value, VentrixConfig.RenderingCentralNodeSize.Value, VentrixConfig.RenderingCentralNodeSize.Value), pulseDuration);

        if (!VentrixConfig.RenderingUseDirectionalNodes.Value)
        {
            return;
        }

        DuctMarkerType directionalType = VentrixConfig.RenderingSpecialDirectionalNodeColors.Value ? result.Type : DuctMarkerType.NormalDuct;
        Vector3 directionalScaleX = new Vector3(VentrixConfig.RenderingDirectionalNodeLength.Value, VentrixConfig.RenderingDirectionalNodeDiameter.Value, VentrixConfig.RenderingDirectionalNodeDiameter.Value);
        Vector3 directionalScaleY= new Vector3(VentrixConfig.RenderingDirectionalNodeDiameter.Value, VentrixConfig.RenderingDirectionalNodeLength.Value, VentrixConfig.RenderingDirectionalNodeDiameter.Value);
        Vector3 directionalScaleZ = new Vector3(VentrixConfig.RenderingDirectionalNodeDiameter.Value, VentrixConfig.RenderingDirectionalNodeDiameter.Value, VentrixConfig.RenderingDirectionalNodeLength.Value);

        if (result.Connections.NegativeX)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(-VentrixConfig.RenderingDirectionalNodeOffset.Value, 0f, 0f), directionalScaleX, pulseDuration);
        }
                
        if (result.Connections.PositiveX)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(VentrixConfig.RenderingDirectionalNodeOffset.Value, 0f, 0f), directionalScaleX, pulseDuration);
        }
                
        if (result.Connections.NegativeY)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(0f, 0f, -VentrixConfig.RenderingDirectionalNodeOffset.Value), directionalScaleZ, pulseDuration);
        }
                
        if (result.Connections.PositiveY)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(0f, 0f, VentrixConfig.RenderingDirectionalNodeOffset.Value), directionalScaleZ, pulseDuration);
        }
                
        if (result.Connections.NegativeZ)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(0f, -VentrixConfig.RenderingDirectionalNodeOffset.Value, 0f), directionalScaleY, pulseDuration);
        }
                
        if (result.Connections.PositiveZ)
        {
            DuctMarkerPool.Instance.CreateMarker(directionalType, position + new Vector3(0f, VentrixConfig.RenderingDirectionalNodeOffset.Value, 0f), directionalScaleY, pulseDuration);
        }
    }
}