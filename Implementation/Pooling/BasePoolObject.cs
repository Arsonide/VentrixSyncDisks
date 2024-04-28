using UnityEngine;

namespace VentVigilante.Implementation.Pooling;

public abstract class BasePoolObject : MonoBehaviour
{
    public virtual void OnCheckout() { }
    public virtual void OnCheckin() { }
}