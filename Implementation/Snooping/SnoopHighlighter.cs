using System.Collections.Generic;
using System.Linq;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentVigilante.Implementation.Common;

namespace VentVigilante.Implementation.Snooping;

public static class SnoopHighlighter
{
    private static bool IsSnooping = false;

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
        IsSnooping = false;
    }

    private static void OnTick()
    {
        if (!IsSnooping)
        {
            for (int i = AllList.Count - 1; i >= 0; --i)
            {
                Actor snooped = AllList[i];
                RemoveSnoopedActor(snooped);
            }

            return;
        }

        NewRoom room = Player.Instance?.currentDuctSection?.node?.room;

        if (room == null)
        {
            return;
        }
        
        for (int i = AllList.Count - 1; i >= 0; --i)
        {
            Actor snooped = AllList[i];
            
            if (!room.currentOccupants.Contains(snooped))
            {
                RemoveSnoopedActor(snooped);
            }
        }
        
        foreach (Actor occupant in room.currentOccupants)
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
        IsSnooping = GetPlayerSnooping();
    }
    
    private static bool GetPlayerSnooping()
    {
        if (Player.Instance == null)
        {
            return false;
        }
        
        if (!Player.Instance.inAirVent)
        {
            return false;
        }

        AirDuctGroup.AirDuctSection section = Player.Instance.currentDuctSection;

        if (section == null)
        {
            return false;
        }

        if (section.peekSection)
        {
            return true;
        }
        
        VentHelpers.GetVentInformation(section, ref Neighbors, ref Vents);
        return Vents.Count > 0;
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