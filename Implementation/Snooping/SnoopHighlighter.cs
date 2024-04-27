using System.Collections.Generic;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Snooping;

public static class SnoopHighlighter
{
    private static NewRoom SnoopingRoom = null;

    private static List<AirDuctGroup.AirDuctSection> Neighbors = new List<AirDuctGroup.AirDuctSection>();
    private static List<AirDuctGroup.AirVent> Vents = new List<AirDuctGroup.AirVent>();
    
    private static List<Actor> AllList = new List<Actor>();
    private static HashSet<int> HumanSet = new HashSet<int>();
    private static HashSet<int> MachineSet = new HashSet<int>();

    public static void Initialize()
    {
        Reset();

        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;

        Timer.OnTick -= OnTick;
        Timer.OnTick += OnTick;
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
        if (SnoopingRoom == null)
        {
            for (int i = AllList.Count - 1; i >= 0; --i)
            {
                Actor snooped = AllList[i];
                RemoveSnoopedActor(snooped);
            }

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
    }

    public static bool IsSnoopedActor(Actor actor)
    {
        return actor.isMachine ? MachineSet.Contains(actor.GetHashCode()) : HumanSet.Contains(actor.GetHashCode());
    }
    
    private static void AddSnoopedActor(Actor actor)
    {
        if (actor.isMachine)
        {
            AddSnoopedActor(actor, ref MachineSet);
        }
        else
        {
            AddSnoopedActor(actor, ref HumanSet);
        }
    }
    
    private static void AddSnoopedActor(Actor actor, ref HashSet<int> set)
    {
        if (actor.isPlayer)
        {
            return;
        }
        
        int code = actor.GetHashCode();
        
        if (set.Contains(code))
        {
            return;
        }

        AllList.Add(actor);
        set.Add(code);
        OnAddSnoopedActor(actor);
    }

    private static void RemoveSnoopedActor(Actor actor)
    {
        if (actor.isMachine)
        {
            RemoveSnoopedActor(actor, ref MachineSet);
        }
        else
        {
            RemoveSnoopedActor(actor, ref HumanSet);
        }
    }
    
    private static void RemoveSnoopedActor(Actor actor, ref HashSet<int> set)
    {
        int code = actor.GetHashCode();
        
        if (!set.Contains(code))
        {
            return;
        }
        
        AllList.Remove(actor);
        set.Remove(code);
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
    }
    
    private static NewRoom GetPlayerSnoopingRoom()
    {
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

        if (section.peekSection)
        {
            return section.node?.room;
        }
        
        VentHelpers.GetVentInformation(section, ref Neighbors, ref Vents);

        if (Vents.Count > 0)
        {
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

            NewRoom sectionRoom = section?.node?.room;

            if (sectionRoom != null)
            {
                return sectionRoom;
            }
        }

        return null;
    }
    
    public static void RefreshActorOutline(Actor actor)
    {
        bool shouldBeOutlined = actor.outline.outlineActive || IsSnoopedActor(actor);

        foreach (MeshRenderer mesh in actor.outline.meshesToOutline)
        {
            if (mesh == null)
            {
                continue;
            }

            mesh.gameObject.layer = shouldBeOutlined ? 30 : 24;
        }
    }
}