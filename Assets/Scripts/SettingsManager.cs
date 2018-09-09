using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class SettingsManager : MonoBehaviour {

    public static SettingsManager instance;

    public static Dictionary<string, string> UserSettings = new Dictionary<string, string>();
    private string[] _basicSettings = new string[]
    {
        "Language = ru",
        "Resolution = 1024x768"
    };

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    public static void LoadSettings()
    {
        string _settingsPath = GameLogic.SettingsPath;

        if (!File.Exists(_settingsPath))
        {
            File.Create(_settingsPath).Close();
            int i;
            using (StreamWriter sw = new StreamWriter(_settingsPath, false, System.Text.Encoding.Default))
            {
                for(i = 0; i < instance._basicSettings.Length; i++)
                {
                    sw.WriteLine(instance._basicSettings[i]);
                }
            }
        }

        using (StreamReader sr = new StreamReader(_settingsPath, System.Text.Encoding.Default))
        {
            string line;
            string[] splitLine = new string[2];
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace(" ", "");
                splitLine = line.Split('=');
                AddSettings(splitLine[0], splitLine[1]);
            }
        }
    }
    private static bool AddSettings(string Name, string Value)
    {
        if (UserSettings.ContainsKey(Name))
            return false;

        UserSettings.Add(Name, Value);

        return true;
    }
}
