using UnityEngine;
using VentVigilante.Implementation.Pooling;

namespace VentVigilante.Implementation.Mapping;

public class EcholocationPulsePool : BasePoolManager<EcholocationPulse>
{
    public static EcholocationPulsePool Instance;
    public WaitForSeconds PulseDelay = null;

    protected override void SetupManager()
    {
        base.SetupManager();
        
        if (PulseDelay == null)
        {
            PulseDelay = new WaitForSeconds(0.05f);
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