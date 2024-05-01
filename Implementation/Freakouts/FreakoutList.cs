using System;
using System.Collections.Generic;

namespace VentrixSyncDisks.Implementation.Freakouts;

[Serializable]
public class FreakoutList
{
    public List<Freakout> Active = new List<Freakout>();
    public HashSet<int> Hourly = new HashSet<int>();
}