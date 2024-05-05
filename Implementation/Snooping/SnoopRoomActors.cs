using System.Collections.Generic;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopRoomActors
{
    private static Stack<SnoopActor> ActorPool = new Stack<SnoopActor>();
    
    private static SnoopActor GetActorFromPool(Actor actor)
    {
        SnoopActor snoopActor = ActorPool.Count > 0 ? ActorPool.Pop() : new SnoopActor();
        snoopActor.Initialize(actor);
        return snoopActor;
    }
    
    private static void ReturnActorToPool(SnoopActor actor)
    {
        actor.Uninitialize();
        ActorPool.Push(actor);
    }
    
    public bool Initialized { get; private set; }
    private NewRoom _room;

    private Dictionary<int, SnoopActor> _idToActor = new Dictionary<int, SnoopActor>();
    private List<SnoopActor> _actors = new List<SnoopActor>();
    
    public void Initialize(NewRoom room)
    {
        if (Initialized)
        {
            return;
        }

        if (room == null)
        {
            return;
        }

        Initialized = true;
        _room = room;

        CacheRoomOccupants();
    }

    public void Uninitialize()
    {
        if (!Initialized)
        {
            return;
        }

        Initialized = false;
        _room = null;

        ClearActors();
    }

    private void CacheRoomOccupants()
    {
        foreach (Actor actor in _room.currentOccupants)
        {
            AddActor(actor);
        }
    }

    private void ClearActors()
    {
        for (int i = _actors.Count - 1; i >= 0; --i)
        {
            RemoveActor(_actors[i].Actor);
        }
    }
    
    public void AddActor(Actor actor)
    {
        if (actor.isMachine || actor.isPlayer)
        {
            return;
        }
        
        SnoopActor snoopActor = GetActorFromPool(actor);
        _actors.Add(snoopActor);
        _idToActor[snoopActor.ID] = snoopActor;
    }

    public void RemoveActor(Actor actor)
    {
        if (actor.isMachine || actor.isPlayer)
        {
            return;
        }
        
        int id = actor.gameObject.GetInstanceID();
        
        if (!_idToActor.TryGetValue(id, out SnoopActor snoopActor))
        {
            return;
        }
        
        _actors.Remove(snoopActor);
        _idToActor.Remove(id);
        ReturnActorToPool(snoopActor);
    }

    public bool TryGetSnoopActor(Actor actor, out SnoopActor snoopActor)
    {
        return _idToActor.TryGetValue(actor.gameObject.GetInstanceID(), out snoopActor);
    }
}