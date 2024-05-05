namespace VentrixSyncDisks.Implementation.Snooping;

public abstract class SnoopRoomBase
{
    public bool Initialized { get; protected set; }
    protected NewRoom Room;

    public void Initialize(NewRoom room)
    {
        if (Initialized || room == null)
        {
            return;
        }

        Initialized = true;
        Room = room;
        OnRoomInitialized();
    }

    public void Uninitialize()
    {
        if (!Initialized)
        {
            return;
        }

        Initialized = false;
        OnRoomUninitialized();
        Room = null;
    }

    protected virtual void OnRoomInitialized() { }
    protected virtual void OnRoomUninitialized() { }
}