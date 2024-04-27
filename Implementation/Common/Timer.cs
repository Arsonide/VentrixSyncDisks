﻿using System;
using UnityEngine;

namespace VentVigilante.Implementation.Common;

public class Timer : MonoBehaviour
{
    public static Action OnTick;
    private static Timer Instance;
    private const float TICK_TIME = 1f;
    private float _timer = TICK_TIME;

    public static void Create()
    {
        if (Instance != null)
        {
            return;
        }

        GameObject go = new GameObject("Timer");
        go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        Timer timer = go.AddComponent<Timer>();
        DontDestroyOnLoad(go);
        Instance = timer;
    }

    private void OnEnable()
    {
        _timer = TICK_TIME;
    }

    private void OnDisable()
    {
        _timer = TICK_TIME;
    }
    
    private void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
        {
            OnTick?.Invoke();
            _timer = TICK_TIME;
        }
    }
}