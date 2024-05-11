using System.Collections.Generic;
using BepInEx.Logging;
using UnityEngine;

namespace VentrixSyncDisks.Implementation.Common;

public static class Utilities
{
    public const bool DEBUG_BUILD = false;
    
    public static void Log(string message, LogLevel level = LogLevel.Info)
    {
        // Debug does not appear, presumably because it's for some functionality we don't have. We'll use it to filter based on DEBUG_BUILD instead.
        if (level == LogLevel.Debug)
        {
#pragma warning disable CS0162

            if (DEBUG_BUILD)
            {
                level = LogLevel.Info;
            }
            else
            {
                return;
            }

#pragma warning restore CS0162
        }

        VentrixPlugin.Log.Log(level, message);
    }

    public static int InverseMultiplierDescription(float multiplier, string lowerDescription, string higherDescription, out string description)
    {
	    if (multiplier < 1)
	    {
		    description = lowerDescription;
		    return Mathf.RoundToInt((1 / multiplier) * 100);
	    }

	    description = higherDescription;
	    return Mathf.RoundToInt((multiplier / 1) * 100);
    }
    
    public static int DirectMultiplierDescription(float multiplier, string lowerDescription, string higherDescription, out string description)
    {
	    if (multiplier < 1)
	    {
		    description = lowerDescription;
		    return Mathf.RoundToInt((1 - multiplier) * 100);
	    }

	    description = higherDescription;
	    return Mathf.RoundToInt((multiplier - 1) * 100);
    }

    public static Color HexToColor(string hex)
    {
        if (hex.StartsWith("#"))
        {
            hex = hex.Substring(1);
        }

        if (hex.Length != 6 && hex.Length != 8)
        {
            return Color.white;
        }

        byte a = 255;

        if (!byte.TryParse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out byte r) ||
            !byte.TryParse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out byte g) ||
            !byte.TryParse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out byte b))
        {
            return Color.white;
        }

        if (hex.Length == 8 && !byte.TryParse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null, out a))
        {
            return Color.white;
        }

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
    
    public static bool RoomsEqual(NewRoom a, NewRoom b)
    {
        bool aNull = a == null;
        bool bNull = b == null;
        
        if (aNull && bNull)
        {
            return false;
        }

        int aID = aNull ? -1 : a.GetInstanceID();
        int bID = bNull ? -1 : b.GetInstanceID();
        
        return aID == bID;
    }
    
#region Vent Related Utilities
	
	public static Vector3 AirDuctToPosition(AirDuctGroup.AirDuctSection airDuct)
	{
		return CityData.Instance.NodeToRealpos(airDuct.node.nodeCoord + new Vector3(0f, 0f, airDuct.level * 2f + 1f) / 6f) + new Vector3(0f, InteriorControls.Instance.airDuctYOffset, 0f);
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

    /// <summary>
    /// I did not write this convoluted logic, I needed to appropriate it to optimize it a little bit though.
    /// </summary>
    public static void GetVentInformation(AirDuctGroup.AirDuctSection thisDuct, ref List<AirDuctGroup.AirDuctSection> neighbors, ref List<Vector3Int> neighborOffsets, ref List<AirDuctGroup.AirVent> vents)
    {
	    neighbors.Clear();
	    neighborOffsets.Clear();
	    vents.Clear();

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
			    neighborOffsets.Add(offset);
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
    
    private static NewNode PositionToNode(Vector3 position)
    {
	    Vector3Int positionInt = CityData.Instance.RealPosToNodeInt(position);
        
	    if (!PathFinder.Instance.nodeMap.TryGetValue(positionInt, out NewNode node))
	    {
		    return Toolbox.Instance.GetNearestGroundLevelOutside(position);
	    }

	    return node;
    }

#endregion
}