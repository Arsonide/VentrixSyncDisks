using System.Collections.Generic;
using UnityEngine;
using BepInEx.Logging;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Pooling;

public class BasePoolManager<T> where T : BasePoolObject
{
    private T _baseObject = null;
    private string _typeName = string.Empty;
    
    private readonly List<T> _availableObjects = new List<T>();
    private readonly List<T> _allObjects = new List<T>();

    public virtual void Initialize()
    {
        SetupManager();
        _typeName = typeof(T).Name;
        
        _baseObject = CreateBaseObject();
        GameObject go = _baseObject.gameObject;
        go.name = _typeName;
        Transform t = go.transform;
        t.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        Object.DontDestroyOnLoad(go);
        go.SetActive(false);
    }

    public virtual void Uninitialize()
    {
        for (int i = _allObjects.Count - 1; i >= 0; --i)
        {
            T poolObject = _allObjects[i];

            if (poolObject.gameObject.activeSelf)
            {
                poolObject.gameObject.SetActive(false);
            }

            Object.Destroy(poolObject.gameObject);
        }
    }

    protected virtual void SetupManager()
    {
        
    }
    
    protected virtual T CreateBaseObject()
    {
        return null;
    }

    public T CheckoutPoolObject()
    {
        T copyObject;
        int lastIndex = _availableObjects.Count - 1;

        if (lastIndex >= 0)
        {
            copyObject = _availableObjects[lastIndex];
            _availableObjects.RemoveAt(lastIndex);
            copyObject.gameObject.SetActive(true);
        }
        else
        {
            copyObject = Object.Instantiate(_baseObject);
            Object.DontDestroyOnLoad(copyObject.gameObject);
            copyObject.gameObject.SetActive(true);
            ConfigureCopyObject(ref copyObject);
            _allObjects.Add(copyObject);
        }
        
        copyObject.OnCheckout();
        return copyObject;
    }
    
    protected virtual void ConfigureCopyObject(ref T copyObject)
    {
        
    }

    public void CheckinPoolObject(T poolObject)
    {
        poolObject.gameObject.SetActive(false);
        _availableObjects.Add(poolObject);
        poolObject.OnCheckin();
    }
}