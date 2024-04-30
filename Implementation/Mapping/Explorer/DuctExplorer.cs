using System.Collections.Generic;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Mapping.Explorer;

public class DuctExplorer
{
    private readonly Queue<AirDuctGroup.AirDuctSection> _queue = new Queue<AirDuctGroup.AirDuctSection>();
    private readonly HashSet<Vector3Int> _visited = new HashSet<Vector3Int>();
    private readonly List<DuctExplorerTick> _results = new List<DuctExplorerTick>();

    private List<AirDuctGroup.AirDuctSection> _neighbors = new List<AirDuctGroup.AirDuctSection>();
    private List<AirDuctGroup.AirVent> _vents = new List<AirDuctGroup.AirVent>();

    public void Reset()
    {
        _queue.Clear();
        _visited.Clear();
        _results.Clear();
        _neighbors.Clear();
        _vents.Clear();
    }
    
    public void StartExploration(AirDuctGroup.AirDuctSection startSection)
    {
        _queue.Enqueue(startSection);
        _visited.Add(startSection.duct);
    }

    // Recursively tick over everything, but we're making the tick manual so we can do it over time.
    public List<DuctExplorerTick> TickExploration(AirDuctGroup.AirDuctSection duct)
    {
        _results.Clear();
        
        if (_queue.Count <= 0)
        {
            return _results;
        }

        int nodesInCurrentLevel = _queue.Count;

        for (int i = 0; i < nodesInCurrentLevel; i++)
        {
            AirDuctGroup.AirDuctSection currentDuct = _queue.Dequeue();
            VentHelpers.GetVentInformation(currentDuct, ref _neighbors, ref _vents);
            DuctExplorerConnections connections = new DuctExplorerConnections();
            
            foreach (AirDuctGroup.AirDuctSection neighbor in _neighbors)
            {
                connections.AddConnection(currentDuct.node.nodeCoord, neighbor.node.nodeCoord);
                
                if (_visited.Contains(neighbor.duct))
                {
                    continue;
                }
                
                _queue.Enqueue(neighbor);
                _visited.Add(neighbor.duct);
            }

            _results.Add(new DuctExplorerTick(currentDuct, _vents.Count > 0, connections));
        }

        return _results;
    }
}