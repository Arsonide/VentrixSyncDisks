using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Implementation.Snooping;

public static class SnoopManager
{
    public static bool IsSnooping;
    public static NewRoom SnoopingRoom;

    private static List<AirDuctGroup.AirDuctSection> _neighbors = new List<AirDuctGroup.AirDuctSection>();
    private static List<Vector3Int> _neighborOffsets = new List<Vector3Int>();
    private static List<AirDuctGroup.AirVent> _vents = new List<AirDuctGroup.AirVent>();
    
    public static void Initialize()
    {
        Reset();

        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;
        
        SnoopWarp.Initialize();
    }

    public static void Uninitialize()
    {
        Reset();
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        SnoopWarp.Uninitialize();
    }

    private static void OnAfterLoad(object sender, SaveGameArgs args)
    {
        Reset();
    }
    
    public static void Reset()
    {
        IsSnooping = false;
        SnoopingRoom = null;
    }

    public static void RefreshSnoopingState()
    {
        int level = DiskRegistry.SnoopingDisk.Level;
        
        if (level <= 0)
        {
            IsSnooping = false;
            SnoopingRoom = null;
            return;
        }

        NewRoom oldRoom = SnoopingRoom;
        NewRoom newRoom = GetPlayerSnoopingRoom(level);

        if (Utilities.RoomsEqual(oldRoom, newRoom))
        {
            return;
        }

        SnoopingRoom = newRoom;
        IsSnooping = SnoopingRoom != null;

        if (newRoom != null)
        {
            OnEnterSnooping(newRoom, level);
        }
    }
    
    private static NewRoom GetPlayerSnoopingRoom(int level)
    {
        if (level <= 0)
        {
            return null;
        }
        
        if (Player.Instance == null)
        {
            return null;
        }
        
        if (!Player.Instance.inAirVent)
        {
            return null;
        }

        AirDuctGroup.AirDuctSection section = Player.Instance.currentDuctSection;

        if (section == null)
        {
            return null;
        }

        if (section.peekSection && VentrixConfig.SnoopingCanSnoopPeeks.GetLevel(level))
        {
            return section.node?.room;
        }
        
        Utilities.GetVentInformation(section, ref _neighbors, ref _neighborOffsets, ref _vents);

        if (_vents.Count <= 0)
        {
            return null;
        }

        // Apparently parts of vent an be unassigned, sometimes...not all the time. Just search a variety of sources for a stupid room.
        AirDuctGroup.AirVent vent = _vents[0];
        NewRoom roomNode = vent?.roomNode?.room;

        if (roomNode != null)
        {
            return roomNode;
        }
            
        NewRoom ventRoom = vent?.room;

        if (ventRoom != null)
        {
            return ventRoom;
        }

        return section.node?.room;
    }

    public static void OnActorRoomChanged(Actor actor)
    {
        if (!IsSnooping || actor.isPlayer || actor.isMachine)
        {
            return;
        }
        
        if (!Utilities.RoomsEqual(actor.currentRoom, SnoopingRoom))
        {
            return;
        }
        
        if (!VentrixConfig.SnoopingCanSnoopCivilians.GetLevel(DiskRegistry.SnoopingDisk.Level))
        {
            return;
        }
        
        SnoopHighlighter highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
        highlight.Setup(SnoopingRoom, actor);
    }

    private static void OnEnterSnooping(NewRoom room, int level)
    {
        if (VentrixConfig.SnoopingCanSnoopCivilians.GetLevel(level))
        {
            SnoopActorsInRoom(room);
        }
        
        if (VentrixConfig.SnoopingCanSnoopSecurity.GetLevel(level))
        {
            SnoopSecurityInRoom(room);
        }
    }

    private static void SnoopActorsInRoom(NewRoom room)
    {
        foreach (Actor actor in room.currentOccupants)
        {
            SnoopHighlighter highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
            highlight.Setup(room, actor);
        }
    }

    private static void SnoopSecurityInRoom(NewRoom room)
    {
        foreach (Il2CppSystem.Collections.Generic.KeyValuePair<InteractablePreset.SpecialCase, Il2CppSystem.Collections.Generic.List<Interactable>> pair in room.specialCaseInteractables)
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

                        SnoopHighlighter highlight;
                        
                        switch (pair.Key)
                        {
                            case InteractablePreset.SpecialCase.securityCamera:
                                highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
                                highlight.Setup(room, interactable.controller.isActor);
                                break;
                            case InteractablePreset.SpecialCase.sentryGun:
                                highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
                                highlight.Setup(room, interactable.controller.securitySystem.rend.gameObject);
                                break;
                            case InteractablePreset.SpecialCase.gasReleaseSystem:
                                highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
                                highlight.Setup(room, interactable.controller.gameObject);
                                break;
                            case InteractablePreset.SpecialCase.otherSecuritySystem:
                                if (!interactable.preset.presetName.ToLowerInvariant().Contains("scout"))
                                {
                                    continue;
                                }

                                GameObject scoutObject = interactable.controller.gameObject;
                                
                                // Hopefully they never change this hierarchy. These references aren't stored anywhere easily accessible.
                                Transform scoutTransform = scoutObject.transform;
                                Transform laserTransform = scoutTransform?.GetChild(0)?.GetChild(0);
                                GameObject laserObject = laserTransform?.gameObject;

                                highlight = SnoopHighlighterPool.Instance.CheckoutPoolObject();
                                highlight.Setup(room, scoutObject, laserObject);
                                
                                break;
                        }
                    }
                    
                    break;
            }
        }
    }
}