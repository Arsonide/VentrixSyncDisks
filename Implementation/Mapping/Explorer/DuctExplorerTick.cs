namespace VentrixSyncDisks.Implementation.Mapping.Explorer;

public struct DuctExplorerTick
{
    public readonly AirDuctGroup.AirDuctSection Duct;
    public readonly DuctMarkerType Type;
    public readonly DuctExplorerConnections Connections;
    
    public DuctExplorerTick(AirDuctGroup.AirDuctSection duct, bool hasVent, DuctExplorerConnections connections)
    {
        Duct = duct;
        
        if (hasVent)
        {
            Type = DuctMarkerType.NormalVent;
        }
        else if (Duct.peekSection)
        {
            Type = DuctMarkerType.PeekDuct;
        }
        else
        {
            Type = DuctMarkerType.NormalDuct;
        }

        Connections = connections;
    }
}