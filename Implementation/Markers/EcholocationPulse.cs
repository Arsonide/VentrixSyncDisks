using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UniverseLib.Runtime.Il2Cpp;
using VentVigilante.Implementation.Common;
using VentVigilante.Implementation.Markers;

namespace VentVigilante.Implementation.Renderers;

public class EcholocationPulse : MonoBehaviour
{
    private Coroutine _pulseCoroutine;

    public void StopPulse()
    {
        if (_pulseCoroutine != null)
        {
            StopCoroutine(_pulseCoroutine);
            _pulseCoroutine = null;
        }
    }

    public void StartPulse(AirDuctGroup.AirDuctSection startDuct)
    {
        StopPulse();
        _pulseCoroutine = StartCoroutine(PulseRoutine(startDuct).WrapToIl2Cpp());
    }
    
    private IEnumerator PulseRoutine(AirDuctGroup.AirDuctSection startDuct)
    {
        AirDuctExplorer explorer = new AirDuctExplorer();
        explorer.StartExploration(startDuct);

        for (int i = 0; i < 50; ++i)
        {
            List<AirDuctExplorer.TickResult> results = explorer.TickExploration(startDuct);

            foreach (AirDuctExplorer.TickResult result in results)
            {
                Vector3 position = VentHelpers.AirDuctToPosition(result.AirDuct);
                DuctMarkerPool.Instance.CreateMarker(result.Type, position, 0.5f);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}