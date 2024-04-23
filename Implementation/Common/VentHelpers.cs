using System.Collections.Generic;
using UnityEngine;

namespace VentVigilante.Implementation.Common;

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
}