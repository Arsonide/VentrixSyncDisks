using VentVigilante.Implementation.Markers;
using VentVigilante.Implementation.Renderers;

namespace VentVigilante.Implementation.Common;

using System.Collections.Generic;
using UnityEngine;

public class AirDuctExplorer
{
    private Queue<AirDuctGroup.AirDuctSection> queue = new Queue<AirDuctGroup.AirDuctSection>();
    private HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
    private static List<AirDuctGroup.AirDuctSection> thisTickSections = new List<AirDuctGroup.AirDuctSection>();
    private static List<AirDuctGroup.AirDuctSection> workingNeighbors = new List<AirDuctGroup.AirDuctSection>();
    private static List<AirDuctGroup.AirVent> workingVents = new List<AirDuctGroup.AirVent>();
    
    public class TickResult
    {
        public AirDuctGroup.AirDuctSection AirDuct;
        public int ConnectionCount;
        public DuctMarkerType Type;

        public TickResult(AirDuctGroup.AirDuctSection airDuct, bool hasVent, int connectionCount)
        {
            AirDuct = airDuct;
            ConnectionCount = connectionCount;
            Type = GetColorBasedOnConnections(hasVent, connectionCount);
        }

        private DuctMarkerType GetColorBasedOnConnections(bool hasVent, int count)
        {
            if (hasVent)
            {
                return DuctMarkerType.NormalVent;
            }

            if (AirDuct.peekSection)
            {
                return DuctMarkerType.PeekDuct;
            }

            return DuctMarkerType.NormalDuct;
        }
    }

    public void StartExploration(AirDuctGroup.AirDuctSection startSection)
    {
        queue.Enqueue(startSection);
        visited.Add(startSection.duct); // Starting point has 0 connections initially
    }

    public List<TickResult> TickExploration(AirDuctGroup.AirDuctSection airDuct)
    {
        List<TickResult> results = new List<TickResult>();
        
        if (queue.Count == 0)
        {
            return results;
        }

        int nodesInCurrentLevel = queue.Count;

        for (int i = 0; i < nodesInCurrentLevel; i++)
        {
            AirDuctGroup.AirDuctSection currentDuct = queue.Dequeue();
            VentHelpers.GetVentInformation(currentDuct, ref workingNeighbors, ref workingVents);

            foreach (AirDuctGroup.AirDuctSection neighbor in workingNeighbors)
            {
                if (visited.Contains(neighbor.duct))
                {
                    continue;
                }
                
                queue.Enqueue(neighbor);
                visited.Add(neighbor.duct); // Mark as visited and store connection count
            }

            // Create a TickResult for the current duct and add it to the results list
            results.Add(new TickResult(currentDuct, workingVents.Count > 0, workingNeighbors.Count));
        }

        return results;
    }
}