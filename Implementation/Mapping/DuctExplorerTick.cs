namespace VentVigilante.Implementation.Mapping;

public struct DuctExplorerTick
{
    public readonly AirDuctGroup.AirDuctSection Duct;
    public readonly DuctMarkerType Type;

    public DuctExplorerTick(AirDuctGroup.AirDuctSection duct, bool hasVent, int connections)
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
    }
}