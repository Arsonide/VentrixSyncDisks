using System.Collections.Generic;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Common;

public static class VentHelpers
{
    public static NewNode PositionToNode(Vector3 position)
    {
        Vector3Int positionInt = CityData.Instance.RealPosToNodeInt(position);
        
        if (!PathFinder.Instance.nodeMap.TryGetValue(positionInt, out NewNode node))
        {
            return Toolbox.Instance.GetNearestGroundLevelOutside(position);
        }

        return node;
    }
    
    public static Vector3 AirDuctToPosition(AirDuctGroup.AirDuctSection airDuct)
    {
        return CityData.Instance.NodeToRealpos(airDuct.node.nodeCoord + new Vector3(0f, 0f, (float)airDuct.level * 2f + 1f) / 6f) + new Vector3(0f, InteriorControls.Instance.airDuctYOffset, 0f);
    }
    
    public static AirDuctGroup.AirDuctSection PositionToAirDuct(Vector3 position)
    {
        const float AIR_DUCT_SIZE = 1f;
        NewNode node = PositionToNode(position);

        if (node == null)
        {
            return null;
        }

        foreach (AirDuctGroup.AirDuctSection airDuct in node.airDucts)
        {
            Vector3 airDuctPosition = AirDuctToPosition(airDuct);
            float distance = Vector3.Distance(position, airDuctPosition);
            
            if (distance <= AIR_DUCT_SIZE)
            {
                return airDuct;
            }
        }

        return null;
    }
    
    public static IEnumerable<AirDuctGroup.AirDuctSection> PositionToAirDucts(Vector3 position, float radius)
    {
        NewNode node = PositionToNode(position);

        if (node == null)
        {
            yield break;
        }

        foreach (AirDuctGroup.AirDuctSection airDuct in node.airDucts)
        {
            Vector3 airDuctPosition = AirDuctToPosition(airDuct);
            float distance = Vector3.Distance(position, airDuctPosition);
            
            if (distance <= radius)
            {
                yield return airDuct;
            }
        }
    }

    /// <summary>
    /// I did not write this convoluted logic, I needed to appropriate it to optimize it a little bit though.
    /// </summary>
    public static void GetVentInformation(AirDuctGroup.AirDuctSection thisDuct, ref List<AirDuctGroup.AirDuctSection> neighbors, ref List<AirDuctGroup.AirVent> vents)
    {
	    vents.Clear();
	    neighbors.Clear();

	    foreach (Vector3Int offset in CityData.Instance.offsetArrayX6)
	    {
		    Vector3Int offsetCoordinate = thisDuct.duct + offset;

		    if (!thisDuct.node.building.ductMap.TryGetValue(offsetCoordinate, out AirDuctGroup.AirDuctSection offsetDuct))
		    {
			    continue;
		    }

		    bool isNeighbor = offset == thisDuct.next || offset == thisDuct.previous || offsetDuct.next == -offset || offsetDuct.previous == -offset;

		    if (isNeighbor && !neighbors.Contains(offsetDuct))
		    {
			    neighbors.Add(offsetDuct);
		    }
	    }
	    
	    if (thisDuct.level == 2)
	    {
		    AirDuctGroup.AirVent airVent = FindCeilingVent(thisDuct.node);

		    if (airVent != null && !vents.Contains(airVent))
		    {
			    vents.Add(airVent);
		    }

		    return;
	    }
	    
	    foreach (Vector2Int lateralOffset in CityData.Instance.offsetArrayX4)
	    {
		    Vector3Int lateralOffset2D = new Vector3Int(lateralOffset.x, lateralOffset.y, 0);
		    Vector3Int foundCoordinate = thisDuct.node.nodeCoord + lateralOffset2D;

		    if (!PathFinder.Instance.nodeMap.TryGetValue(foundCoordinate, out NewNode foundNode))
		    {
			    continue;
		    }

		    AirDuctGroup.AirVent airVent = FindWallVent(foundNode, thisDuct.node);

		    if (airVent == null)
		    {
			    continue;
		    }

		    if (airVent.ventType == NewAddress.AirVent.wallUpper && thisDuct.level == 1)
		    {
			    if (!vents.Contains(airVent))
			    {
				    vents.Add(airVent);
			    }
		    }
		    else if (airVent.ventType == NewAddress.AirVent.wallLower && thisDuct.level == 0)
		    {
			    if (!vents.Contains(airVent))
			    {
				    vents.Add(airVent);
			    }
		    }
	    }
    }

    private static AirDuctGroup.AirVent FindCeilingVent(NewNode ductNode)
    {
	    foreach (AirDuctGroup.AirVent vent in ductNode.room.airVents)
	    {
		    if (vent.node.nodeCoord == ductNode.nodeCoord && vent.ventType == NewAddress.AirVent.ceiling)
		    {
			    return vent;
		    }
	    }

	    return null;
    }
    
    private static AirDuctGroup.AirVent FindWallVent(NewNode foundNode, NewNode ductNode) // foundNode, thisDuct.node
    {
	    foreach (AirDuctGroup.AirVent vent in foundNode.room.airVents)
	    {
		    if (vent.node.nodeCoord == ductNode.nodeCoord && vent.roomNode.nodeCoord == foundNode.nodeCoord)
		    {
			    return vent;
		    }
	    }

	    return null;
    }
}