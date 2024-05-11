using System.IO;
using System.Reflection;
using System.Text.Json;
using SOD.Common;
using SOD.Common.Helpers;
using UnityEngine;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Freakouts;

public static class FreakoutManager
{
    private static FreakoutList _freakouts = new FreakoutList();
    
    public static void Initialize()
    {
        Timer.OnTick -= OnTick;
        Timer.OnTick += OnTick;

        Lib.Time.OnHourChanged -= OnHourChanged;
        Lib.Time.OnHourChanged += OnHourChanged;

        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;

        Lib.SaveGame.OnAfterSave -= OnAfterSave;
        Lib.SaveGame.OnAfterSave += OnAfterSave;
    }

    public static void Uninitialize()
    {
        Timer.OnTick -= OnTick;
        Lib.Time.OnHourChanged -= OnHourChanged;
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterSave -= OnAfterSave;
    }

    private static void OnTick()
    {
        if (Lib.Time.IsGamePaused)
        {
            return;
        }
        
        for (int i = _freakouts.Active.Count - 1; i >= 0; --i)
        {
            Freakout freakout = _freakouts.Active[i];
            freakout.TicksLeft--;

            if (freakout.TicksLeft <= 0 && freakout.TryGetHuman(out Human human))
            {
                ForceSetNerve(human, freakout.NerveTaken);
                _freakouts.Active.RemoveAt(i);
            }
        }
    }

    private static void OnHourChanged(object sender, TimeChangedArgs args)
    {
        _freakouts.Hourly.Clear();
    }
    
    public static void StartFreakout(Human human, int seconds)
    {
        // Only scare people once per hour.
        if (human == null || human.ai == null || _freakouts.Hourly.Contains(human.humanID))
        {
            return;
        }
        
        _freakouts.Active.Add(new Freakout()
        {
            HumanID = human.humanID,
            HumanCache = human,
            NerveTaken = human.currentNerve,
            TicksLeft = seconds,
        });

        _freakouts.Hourly.Add(human.humanID);
        ForceSetNerve(human, 0f);
    }

    private static void ForceSetNerve(Human human, float nerve)
    {
        human.ai.SetPersuit(val: false);
        human.ai.ResetInvestigate();
        human.ai.CancelCombat();
        human.SetNerve(Mathf.Clamp(nerve, 0f, human.maxNerve));
        human.ai.AITick(true, true);
    }

#region Serialization

    private static void ResetDefaults()
    {
        _freakouts = new FreakoutList();
    }
    
    private static void OnAfterSave(object sender, SaveGameArgs args)
    {
        try
        {
            string path = GetSavePath(args);
            
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            
            string json = JsonSerializer.Serialize(_freakouts, options);
            File.WriteAllText(path, json);
        }
        catch
        {
            ResetDefaults();
        }
    }

    private static void OnAfterLoad(object sender, SaveGameArgs args)
    {
        try
        {
            string path = GetSavePath(args);
            string json = File.ReadAllText(path);
            
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IncludeFields = true,
            };
            
            _freakouts = JsonSerializer.Deserialize<FreakoutList>(json, options);
        }
        catch
        {
            ResetDefaults();
        }
    }
    
    private static string GetSavePath(SaveGameArgs args)
    {
        string saveCode = Lib.SaveGame.GetUniqueString(args.FilePath);
        string fileName = $"ModJsonData_{saveCode}_VentrixFreakouts.jsonc";
        return Lib.SaveGame.GetSavestoreDirectoryPath(Assembly.GetExecutingAssembly(), fileName);
    }
    
#endregion
}