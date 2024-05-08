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
    private static NewRoom SnoopingRoom = null;

    private static List<AirDuctGroup.AirDuctSection> Neighbors = new List<AirDuctGroup.AirDuctSection>();
    private static List<Vector3Int> NeighborOffsets = new List<Vector3Int>();
    private static List<AirDuctGroup.AirVent> Vents = new List<AirDuctGroup.AirVent>();

    private static MaterialPropertyBlock FullAlphaBlock = new MaterialPropertyBlock();

    private static SnoopRoomSecurity _securityRoom = new SnoopRoomSecurity();
    private static SnoopRoomActors _actorRoom = new SnoopRoomActors();

    public static bool IsSnooping => SnoopingRoom != null;
    
    public static void Initialize()
    {
        Reset();

        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;

        FullAlphaBlock.SetFloat("_AlphaVal", 1f);
        
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
        SnoopingRoom = null;
    }

    public static void RefreshSnoopingState()
    {
        _securityRoom.Uninitialize();
        _actorRoom.Uninitialize();
        
        int level = DiskRegistry.SnoopingDisk.Level;
        
        if (level <= 0)
        {
            SnoopingRoom = null;
            return;
        }
        
        SnoopingRoom = GetPlayerSnoopingRoom(level);
        
        if (VentrixConfig.SnoopingCanSnoopCivilians.GetLevel(level))
        {
            _actorRoom.Initialize(SnoopingRoom);
        }
        
        if (VentrixConfig.SnoopingCanSnoopSecurity.GetLevel(level))
        {
            _securityRoom.Initialize(SnoopingRoom);
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
        
        VentHelpers.GetVentInformation(section, ref Neighbors, ref NeighborOffsets, ref Vents);

        if (Vents.Count <= 0)
        {
            return null;
        }

        // Apparently parts of vent an be unassigned, sometimes...not all the time. Just search a variety of sources for a stupid room.
        AirDuctGroup.AirVent vent = Vents[0];
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

        return section?.node?.room;
    }

    public static void OnActorRoomChanged(Actor actor)
    {
        if (!_actorRoom.Initialized || actor.isPlayer || actor.isMachine)
        {
            return;
        }

        int snoopID = SnoopingRoom.GetInstanceID();
        
        if (actor.previousRoom.GetInstanceID() == snoopID)
        {
            _actorRoom.RemoveActor(actor);
        }
        else if (actor.currentRoom.GetInstanceID() == snoopID)
        {
            _actorRoom.AddActor(actor);
        }
    }

    public static void OnActorChangedMeshes(Actor actor)
    {
        if (!_actorRoom.Initialized || actor.currentRoom.GetInstanceID() != SnoopingRoom.GetInstanceID() || !_actorRoom.TryGetSnoopActor(actor, out SnoopActor snoopActor))
        {
            return;
        }

        snoopActor.SynchronizeObjects();
    }

    public static void EnforceOutlineLayer(Actor actor)
    {
        if (!_actorRoom.Initialized || actor.currentRoom.GetInstanceID() != SnoopingRoom.GetInstanceID())
        {
            return;
        }
        
        foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
        {
            if (mesh != null)
            {
                mesh.gameObject.layer = SnoopActor.OUTLINE_LAYER;
            }
        }
    }
    
    public static void EnforceOutlineAlpha(Actor actor)
    {
        if (!_actorRoom.Initialized || actor.currentRoom.GetInstanceID() != SnoopingRoom.GetInstanceID())
        {
            return;
        }

        foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
        {
            if (mesh != null)
            {
                mesh.SetPropertyBlock(FullAlphaBlock);
            }
        }
    }
}