using SOD.Common;
using SOD.Common.Helpers;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Implementation.Snooping;

public static class SnoopWarp
{
    private static float _warpDelay = 10f;
    private static float _notificationDelay = 5f;
    
    private static SnoopWarpState _state = SnoopWarpState.None;
    private static float _timer = 0f;
    private static bool _notification = false;

    public static void Initialize()
    {
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;

        _warpDelay = VentrixConfig.SnoopingPassTimeWarpDelay.Value;
        _notificationDelay = VentrixConfig.SnoopingPassTimeNotificationDelay.Value;
        
        Reset();
    }
    
    public static void Uninitialize()
    {
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Reset();
    }

    private static void OnAfterLoad(object sender, SaveGameArgs args)
    {
        Reset();
    }

    private static void Reset()
    {
        SetState(SnoopWarpState.None, true);
    }

    public static void OnUpdate()
    {
        int level = DiskRegistry.SnoopingDisk.Level;
        
        if (level <= 0 || !VentrixConfig.SnoopingCanPassTime.GetLevel(level))
        {
            return;
        }
        
        bool engagingSnoopWarp = Player.Instance.inAirVent && FirstPersonItemController.Instance.currentItem == GameplayControls.Instance.watchItem && SnoopManager.IsSnooping;

        if (!engagingSnoopWarp)
        {
            SetState(SnoopWarpState.None);
            return;
        }

        switch (_state)
        {
            case SnoopWarpState.None:
                SetState(SnoopWarpState.Engaging);

                break;
            case SnoopWarpState.Engaging:
                _timer += UnityEngine.Time.deltaTime;

                if (!_notification && _notificationDelay > 0 && _timer >= _notificationDelay)
                {
                    InterfaceController.Instance.NewGameMessage(InterfaceController.GameMessageType.notification, 0, "Starting to pass time at vent...");
                    _notification = true;
                }
                
                if (_timer >= _warpDelay)
                {
                    SetState(SnoopWarpState.TimeWarping);
                }
                
                break;
            case SnoopWarpState.TimeWarping:
                PushOutPlayerAlarm();
                break;
        }
    }

    private static void SetState(SnoopWarpState state, bool force = false)
    {
        SnoopWarpState newState = state;
        SnoopWarpState oldState = _state;

        if (!force && newState == oldState)
        {
            return;
        }
        
        _state = newState;
        _timer = 0f;
        _notification = false;
        
        if (oldState == SnoopWarpState.TimeWarping && newState != SnoopWarpState.TimeWarping)
        {
            Player.Instance.SetSpendingTimeMode(val: false);
            AudioController.Instance.PlayWorldOneShot(AudioControls.Instance.watchAlarm, Player.Instance, Player.Instance.currentNode, Player.Instance.lookAtThisTransform.position);
            ResetPlayerAlarm();
        }
        
        switch (newState)
        {
            case SnoopWarpState.TimeWarping:
                PushOutPlayerAlarm();
                Player.Instance.SetSpendingTimeMode(true);
                break;
        }
    }

    private static void PushOutPlayerAlarm()
    {
        Player.Instance.alarm = SessionData.Instance.gameTime + 9999f;
    }

    private static void ResetPlayerAlarm()
    {
        Player.Instance.alarm = SessionData.Instance.gameTime - 1f;
    }
}