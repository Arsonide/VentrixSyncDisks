using System.Collections.Generic;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopActor
{
    public const int OUTLINE_LAYER = 30;
    private const int DEFAULT_LAYER = 24;
    
    public Actor Actor;
    public int ID;

    private readonly List<GameObject> _objects = new List<GameObject>();
    
    public void Initialize(Actor actor)
    {
        ClearObjects();
        Actor = actor;
        ID = actor.gameObject.GetInstanceID();
        SynchronizeObjects();
    }
    
    public void Uninitialize()
    {
        ClearObjects();
        Actor = null;
        ID = -1;
    }
    
    public void SynchronizeObjects()
    {
        if (Actor == null)
        {
            return;
        }

        ClearObjects();
        
        foreach (MeshRenderer mesh in Actor.outline.meshesToOutline)
        {
            if (mesh == null)
            {
                continue;
            }
         
            GameObject go = mesh.gameObject;
            go.layer = OUTLINE_LAYER;
            _objects.Add(go);
        }
    }

    public void ClearObjects()
    {
        for (int i = _objects.Count - 1; i >= 0; --i)
        {
            GameObject go = _objects[i];

            if (go != null)
            {
                go.layer = DEFAULT_LAYER;
            }
            
            _objects.RemoveAt(i);
        }
    }
}