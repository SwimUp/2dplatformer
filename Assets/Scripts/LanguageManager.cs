using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public sealed class LanguageManager : MonoBehaviour {

    public static LanguageManager instance;

    public static readonly Dictionary<string, string> TextList = new Dictionary<string, string>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    public static void LoadLanguage()
    {
        if (GameLogic.isLoad)
            return;

        LanguageData lang = new LanguageData();

        string _language = GameLogic.LanguagePath + SettingsManager.UserSettings["Language"] + ".xml";
        if (!File.Exists(_language))
            _language = GameLogic.LanguagePath + "ru.xml";

        lang = (LanguageData)Utility.DeserializeData(_language, typeof(LanguageData));
        int i;
        for (i = 0; i < lang.LanguageList.Count; i++)
        {
            SettingsData data = lang.LanguageList[i];
            AddLanguageText(data.Key, data.Value);
        }
    }
    private static bool AddLanguageText(string id, string text)
    {
        if (TextList.ContainsKey(id))
            return false;

        TextList.Add(id, text);

        return true;
    }
}
[System.Serializable]
public sealed class LanguageData
{
    [System.Xml.Serialization.XmlArray(ElementName = "Data")]
    [System.Xml.Serialization.XmlArrayItem(ElementName = "String")]
    public List<SettingsData> LanguageList = new List<SettingsData>();

    public LanguageData()
    {
    }
}
