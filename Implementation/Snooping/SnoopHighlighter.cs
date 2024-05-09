using System.Collections.Generic;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopHighlighter : BasePoolObject
{
    private const int HIGHLIGHTED_LAYER = 30;
    private const int ACTOR_UNHIGHLIGHTED_LAYER = 24;
    
    private List<GameObject> _highlightObjects = new List<GameObject>();
    private List<int> _highlightLayers = new List<int>();
    private Actor _actor;
    private NewRoom _room;
    private bool _isActor;
    
    public override void OnCheckout()
    {
        base.OnCheckout();
        ResetDefaults();
    }

    public override void OnCheckin()
    {
        base.OnCheckin();
        Cleanup();
        ResetDefaults();
    }

    private void ResetDefaults()
    {
        _highlightObjects.Clear();
        _highlightLayers.Clear();
        _room = null;
        _actor = null;
        _isActor = false;
    }

    public void Setup(NewRoom room, Actor actor)
    {
        _room = room;
        _actor = actor;
        _isActor = true;
    }
    
    public void Setup(NewRoom room, GameObject staticObject)
    {
        _room = room;
        
        if (staticObject != null)
        {
            _highlightObjects.Add(staticObject);
            _highlightLayers.Add(staticObject.layer);
            staticObject.layer = HIGHLIGHTED_LAYER;
        }

        _isActor = false;
    }
    
    public void Setup(NewRoom room, GameObject staticObject, GameObject auxObject)
    {
        _room = room;
        
        if (staticObject != null)
        {
            _highlightObjects.Add(staticObject);
            _highlightLayers.Add(staticObject.layer);
            staticObject.layer = HIGHLIGHTED_LAYER;
        }
        
        if (auxObject != null)
        {
            _highlightObjects.Add(auxObject);
            _highlightLayers.Add(auxObject.layer);
            auxObject.layer = HIGHLIGHTED_LAYER;
        }

        _isActor = false;
    }

    private void Cleanup()
    {
        for (int i = _highlightObjects.Count - 1; i >= 0; --i)
        {
            GameObject go = _highlightObjects[i];

            if (!_isActor)
            {
                if (go != null)
                {
                    go.layer = _highlightLayers[i];
                }

                _highlightObjects.RemoveAt(i);
                _highlightLayers.RemoveAt(i);
            }
            else
            {
                if (go != null)
                {
                    go.layer = _actor != null && _actor.outline.outlineActive ? HIGHLIGHTED_LAYER : ACTOR_UNHIGHLIGHTED_LAYER;
                }

                _highlightObjects.RemoveAt(i);
            }
        }
    }

    private void LateUpdate()
    {
        if (_isActor)
        {
            UpdateActor();
        }
        else
        {
            UpdateStatic();
        }
    }

    private void UpdateActor()
    {
        if (_actor == null || _actor.outline == null || SnoopManager.SnoopingRoom == null || _actor.currentRoom == null || SnoopManager.SnoopingRoom.GetInstanceID() != _actor.currentRoom.GetInstanceID())
        {
            SnoopHighlighterPool.Instance.CheckinPoolObject(this);
            return;
        }

        for (int i = _highlightObjects.Count - 1; i >= 0; --i)
        {
            GameObject go = _highlightObjects[i];
            
            if (go != null)
            {
                go.layer = ACTOR_UNHIGHLIGHTED_LAYER;
            }
            
            _highlightObjects.RemoveAt(i);
        }

        foreach (MeshRenderer renderer in _actor.outline.meshesToOutline)
        {
            if (renderer == null)
            {
                continue;
            }
            
            GameObject go = renderer.gameObject;
            _highlightObjects.Add(go);
            go.layer = HIGHLIGHTED_LAYER;
            
            renderer.SetPropertyBlock(SnoopHighlighterPool.FullAlphaBlock);
        }
    }
    
    private void UpdateStatic()
    {
        if (SnoopManager.SnoopingRoom == null || SnoopManager.SnoopingRoom.GetInstanceID() != _room.GetInstanceID())
        {
            SnoopHighlighterPool.Instance.CheckinPoolObject(this);
        }
    }
}