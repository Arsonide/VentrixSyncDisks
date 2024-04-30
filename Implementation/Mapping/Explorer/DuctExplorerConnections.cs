using UnityEngine;

namespace VentrixSyncDisks.Implementation.Mapping.Explorer;

public struct DuctExplorerConnections
{
    public bool PositiveX;
    public bool PositiveY;
    public bool PositiveZ;
    public bool NegativeX;
    public bool NegativeY;
    public bool NegativeZ;

    public void AddConnection(Vector3Int from, Vector3Int to)
    {
        if (from == to)
        {
            return;
        }
        
        Vector3Int direction = to - from;
        
        if (direction.x > 0)
        {
            PositiveX = true;
        }
        else if (direction.x < 0)
        {
            NegativeX = true;
        }

        if (direction.y > 0)
        {
            PositiveY = true;
        }
        else if (direction.y < 0)
        {
            NegativeY = true;
        }

        if (direction.z > 0)
        {
            PositiveZ = true;
        }
        else if (direction.z < 0)
        {
            NegativeZ = true;
        }
    }
}