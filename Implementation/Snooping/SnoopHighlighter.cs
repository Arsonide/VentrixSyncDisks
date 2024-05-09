using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Pooling;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopHighlighter : BasePoolObject
{
    private const int HIGHLIGHTED_LAYER = 30;
    private const int ACTOR_UNHIGHLIGHTED_LAYER = 24;
    
    private List<GameObject> _highlightObjects = new List<GameObject>();
    private List<int> _highlightLayers = new List<int>();
    
    private bool _isActor;
    private Actor _actor;
    private NewRoom _startRoom;
    
    private void OnEnable()
    {
        Lib.SaveGame.OnBeforeLoad -= OnLoadEvent;
        Lib.SaveGame.OnBeforeLoad += OnLoadEvent;
        Lib.SaveGame.OnAfterLoad -= OnLoadEvent;
        Lib.SaveGame.OnAfterLoad += OnLoadEvent;
    }

    private void OnDisable()
    {
        Lib.SaveGame.OnBeforeLoad -= OnLoadEvent;
        Lib.SaveGame.OnAfterLoad -= OnLoadEvent;
    }

    private void OnLoadEvent(object sender, SaveGameArgs args)
    {
        SnoopHighlighterPool.Instance.CheckinPoolObject(this);
    }
    
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
        
        _isActor = false;
        _actor = null;
        _startRoom = null;
    }

    public void Setup(NewRoom room, Actor actor)
    {
        _isActor = true;
        _actor = actor;
        _startRoom = room;
    }
    
    public void Setup(NewRoom room, GameObject staticObject)
    {
        if (staticObject != null)
        {
            _highlightObjects.Add(staticObject);
            _highlightLayers.Add(staticObject.layer);
            staticObject.layer = HIGHLIGHTED_LAYER;
        }

        _isActor = false;
        _startRoom = room;
    }
    
    public void Setup(NewRoom room, GameObject staticObject, GameObject auxObject)
    {
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
        _startRoom = room;
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
        if (ShouldReturnToPool())
        {
            SnoopHighlighterPool.Instance.CheckinPoolObject(this);
            return;
        }

        if (!_isActor)
        {
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

    private bool ShouldReturnToPool()
    {
        if (!SnoopManager.IsSnooping || SnoopManager.SnoopingRoom == null)
        {
            return true;
        }
        
        int level = DiskRegistry.SnoopingDisk.Level;

        if (level <= 0 || SnoopManager.SnoopingRoom == null)
        {
            return true;
        }

        if (_isActor)
        {
            if (!VentrixConfig.SnoopingCanSnoopCivilians.GetLevel(level))
            {
                return true;
            }
            
            return _actor == null || _actor.outline == null ||  _actor.currentRoom == null || !Utilities.RoomsEqual(SnoopManager.SnoopingRoom, _actor.currentRoom);
        }

        if (!VentrixConfig.SnoopingCanSnoopSecurity.GetLevel(level))
        {
            return true;
        }

        return !Utilities.RoomsEqual(SnoopManager.SnoopingRoom, _startRoom);
    }
}