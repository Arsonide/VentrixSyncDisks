using UnityEngine;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Mapping;

public class EcholocationPulsePool : BasePoolManager<EcholocationPulse>
{
    public static EcholocationPulsePool Instance;
    
    public WaitForSeconds[] PulseDelays = null;

    protected override void SetupManager()
    {
        base.SetupManager();

        if (PulseDelays != null)
        {
            return;
        }

        PulseDelays = new WaitForSeconds[4];

        for (int i = 0, iC = PulseDelays.Length; i < iC; ++i)
        {
            // Index zero is just holding a place. It will use the default value.
            PulseDelays[i] = new WaitForSeconds(VentrixConfig.MappingEcholocationSpeed.GetLevel(i));
        }
    }

    protected override EcholocationPulse CreateBaseObject()
    {
        GameObject pulseObject = new GameObject(nameof(EcholocationPulse));
        EcholocationPulse pulse = pulseObject.AddComponent<EcholocationPulse>();
        
        Transform t = pulseObject.transform;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        t.localScale = Vector3.one;

        Object.DontDestroyOnLoad(pulseObject);
        pulseObject.SetActive(false);

        return pulse;
    }
}