using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Disks;

namespace VentrixSyncDisks.Implementation.Snooping;

public static class SnoopHighlighter
{
    private static NewRoom SnoopingRoom = null;

    private static List<AirDuctGroup.AirDuctSection> Neighbors = new List<AirDuctGroup.AirDuctSection>();
    private static List<AirDuctGroup.AirVent> Vents = new List<AirDuctGroup.AirVent>();
    
    private static List<Actor> AllList = new List<Actor>();
    private static HashSet<int> HumanSet = new HashSet<int>();

    private static MaterialPropertyBlock FullAlphaBlock = new MaterialPropertyBlock();

    private static SnoopRoomSecurity _securityRoom = new SnoopRoomSecurity();
    
    public static void Initialize()
    {
        Reset();

        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;

        Timer.OnTick -= OnTick;
        Timer.OnTick += OnTick;
        
        FullAlphaBlock.SetFloat("_AlphaVal", 1f);
    }

    public static void Uninitialize()
    {
        Reset();
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Timer.OnTick -= OnTick;
    }

    private static void OnAfterLoad(object sender, SaveGameArgs args)
    {
        Reset();
    }
    
    public static void Reset()
    {
        SnoopingRoom = null;
    }

    private static void OnTick()
    {
        if (SnoopingRoom == null || DiskRegistry.SnoopingDisk.Level <= 0)
        {
            ClearSnoopedActors();
            return;
        }
        
        for (int i = AllList.Count - 1; i >= 0; --i)
        {
            Actor snooped = AllList[i];
            
            if (!SnoopingRoom.currentOccupants.Contains(snooped))
            {
                RemoveSnoopedActor(snooped);
            }
        }
        
        foreach (Actor occupant in SnoopingRoom.currentOccupants)
        {
            if (!IsSnoopedActor(occupant))
            {
                AddSnoopedActor(occupant);
            }
        }
        
        // When actors change clothes and stuff the outline gets off.
        EnforceMeshLayers();
    }

    private static void ClearSnoopedActors()
    {
        int count = AllList.Count;

        if (count <= 0)
        {
            return;
        }

        for (int i = count - 1; i >= 0; --i)
        {
            Actor snooped = AllList[i];
            RemoveSnoopedActor(snooped);
        }
    }

    public static bool IsSnoopedActor(Actor actor)
    {
        return HumanSet.Contains(actor.GetHashCode());
    }

    private static void AddSnoopedActor(Actor actor)
    {
        if (actor.isPlayer)
        {
            return;
        }
        
        int code = actor.GetHashCode();
        
        if (HumanSet.Contains(code))
        {
            return;
        }

        AllList.Add(actor);
        HumanSet.Add(code);
        OnAddSnoopedActor(actor);
    }

    private static void RemoveSnoopedActor(Actor actor)
    {
        int code = actor.GetHashCode();
        
        if (!HumanSet.Contains(code))
        {
            return;
        }
        
        AllList.Remove(actor);
        HumanSet.Remove(code);
        OnRemoveSnoopedActor(actor);
    }

    private static void OnAddSnoopedActor(Actor actor)
    {
        RefreshActorOutline(actor);
    }
    
    private static void OnRemoveSnoopedActor(Actor actor)
    {
        RefreshActorOutline(actor);
    }

    public static void RefreshSnoopingState()
    {
        SnoopingRoom = GetPlayerSnoopingRoom();
        
        _securityRoom.Uninitialize();
        _securityRoom.Initialize(SnoopingRoom);
    }
    
    private static NewRoom GetPlayerSnoopingRoom()
    {
        if (DiskRegistry.SnoopingDisk.Level <= 0)
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

        if (section.peekSection && DiskRegistry.SnoopingDisk.Level >= 2)
        {
            return section.node?.room;
        }
        
        VentHelpers.GetVentInformation(section, ref Neighbors, ref Vents);

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
    
    public static void RefreshActorOutline(Actor actor)
    {
        bool snooped = IsSnoopedActor(actor);
        bool shouldBeOutlined = actor.outline.outlineActive || snooped;

        foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
        {
            if (mesh == null)
            {
                continue;
            }

            mesh.gameObject.layer = shouldBeOutlined ? 30 : 24;
            
        }

        if (!snooped)
        {
            return;
        }

        foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
        {
            mesh.SetPropertyBlock(FullAlphaBlock);
        }
    }

    private static void EnforceMeshLayers()
    {
        foreach (Actor actor in AllList)
        {
            foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
            {
                mesh.gameObject.layer = 30;
            }
        }
    }
}