using Il2CppSystem.Collections.Generic;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Snooping;

public class SnoopRoomSecurity
{
    public bool Initialized { get; private set; }
    private NewRoom _room;

    private readonly List<GameObject> _securityObjects = new List<GameObject>();
    private readonly Dictionary<GameObject, int> _securityLayers = new Dictionary<GameObject, int>();

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

        CacheSecurityObjects();
    }
    
    public void Uninitialize()
    {
        if (!Initialized)
        {
            return;
        }
        
        Initialized = false;
        _room = null;
        
        ClearSecurityObjects();
    }

    private void CacheSecurityObjects()
    {
        foreach (KeyValuePair<InteractablePreset.SpecialCase, List<Interactable>> pair in _room.specialCaseInteractables)
        {
            switch (pair.Key)
            {
                case InteractablePreset.SpecialCase.securityCamera:
                case InteractablePreset.SpecialCase.gasReleaseSystem:
                case InteractablePreset.SpecialCase.sentryGun:
                case InteractablePreset.SpecialCase.otherSecuritySystem:
                    foreach(Interactable interactable in pair.Value)
                    {
                        if (interactable == null || interactable.controller == null)
                        {
                            continue;
                        }

                        switch (pair.Key)
                        {
                            case InteractablePreset.SpecialCase.securityCamera:
                            case InteractablePreset.SpecialCase.sentryGun:
                                AddSecurityObject(interactable.controller.securitySystem.rend.gameObject);
                                break;
                            case InteractablePreset.SpecialCase.gasReleaseSystem:
                                AddSecurityObject(interactable.controller.gameObject);
                                break;
                            case InteractablePreset.SpecialCase.otherSecuritySystem:
                                if (!interactable.preset.presetName.ToLowerInvariant().Contains("scout"))
                                {
                                    continue;
                                }

                                GameObject scoutObject = interactable.controller.gameObject;
                                AddSecurityObject(scoutObject);
                                Transform scoutTransform = scoutObject.transform;
                                
                                // Hopefully they never change this hierarchy. These references aren't stored anywhere easily accessible.
                                Transform laserTransform = scoutTransform?.GetChild(0)?.GetChild(0);

                                if (laserTransform != null)
                                {
                                    AddSecurityObject(laserTransform.gameObject);
                                }
                                
                                break;
                        }
                    }
                    
                    break;
            }
        }
    }

    private void ClearSecurityObjects()
    {
        for (int i = _securityObjects.Count - 1; i >= 0; --i)
        {
            RemoveSecurityObject(_securityObjects._items[i]);
        }
        
        // Just to be sure.
        _securityObjects.Clear();
        _securityLayers.Clear();
    }
    
    private void AddSecurityObject(GameObject go)
    {
        _securityObjects.Add(go);
        _securityLayers[go] = go.layer;
        go.layer = 30;
    }
    
    private void RemoveSecurityObject(GameObject go)
    {
        if (!_securityLayers.TryGetValue(go, out int layer))
        {
            return;
        }

        go.layer = layer;
        _securityObjects.Remove(go);
        _securityLayers.Remove(go);
    }
}