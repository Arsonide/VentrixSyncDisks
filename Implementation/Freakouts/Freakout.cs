using System;
using System.Text.Json.Serialization;

namespace VentrixSyncDisks.Implementation.Freakouts;

[Serializable]
public class Freakout
{
    public int HumanID;
    public int TicksLeft;
    public float NerveTaken;

    [JsonIgnore]
    public Human HumanCache;

    public bool TryGetHuman(out Human human)
    {
        if (HumanCache != null)
        {
            human = HumanCache;
            return true;
        }

        return CityData.Instance.citizenDictionary.TryGetValue(HumanID, out human);
    }
}