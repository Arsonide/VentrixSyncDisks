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

    public void AddConnection(Vector3Int offset)
    {
        if (offset.x > 0)
        {
            PositiveX = true;
        }
        else if (offset.x < 0)
        {
            NegativeX = true;
        }

        if (offset.y > 0)
        {
            PositiveY = true;
        }
        else if (offset.y < 0)
        {
            NegativeY = true;
        }

        if (offset.z > 0)
        {
            PositiveZ = true;
        }
        else if (offset.z < 0)
        {
            NegativeZ = true;
        }
    }
}