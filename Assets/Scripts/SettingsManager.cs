using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public sealed class SettingsManager : MonoBehaviour {

    public static SettingsManager instance;

    public static readonly Dictionary<string, string> UserSettings = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    public static void LoadSettings()
    {
        Settings settings = new Settings();

        string _settingsPath = GameLogic.SettingsPath;
        if (!File.Exists(_settingsPath))
        {
            settings.SetDefault();
            Utility.SerializeData(_settingsPath, settings);
        }

        settings = (Settings)Utility.DeserializeData(_settingsPath, typeof(Settings));
        int i;
        for(i = 0; i < settings.SettingsList.Count; i++)
        {
            SettingsData data = settings.SettingsList[i];
            AddSettings(data.Key, data.Value);
        }
        
    }
    private static bool AddSettings(string Name, string Value)
    {
        if (UserSettings.ContainsKey(Name))
            return false;

        UserSettings.Add(Name, Value);

        return true;
    }
    public static void SaveSettings()
    {
        Settings settings = new Settings();

        foreach(KeyValuePair<string,string> d in UserSettings)
        {
            SettingsData data = new SettingsData(d.Key, d.Value);
            settings.SettingsList.Add(data);
        }

        Utility.SerializeData(GameLogic.SettingsPath, settings);
    }
}
[System.Serializable]
public sealed class Settings
{
    [System.Xml.Serialization.XmlArrayItem(ElementName = "Parameter")]
    public List<SettingsData> SettingsList = new List<SettingsData>();

    [System.NonSerialized]
    private readonly Dictionary<string, string> _defaultSettings = new Dictionary<string, string>
    {
        {"Language", "ru" },
        {"Resolution", "1024x768" }
    };

    public Settings()
    {
    }

    public void SetDefault()
    {
        foreach (KeyValuePair<string, string> d in _defaultSettings)
        {
            SettingsList.Add
                (
                    new SettingsData(d.Key, d.Value)
                );
        }
    }
}