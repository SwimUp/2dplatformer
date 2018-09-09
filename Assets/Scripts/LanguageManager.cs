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

        string _language = GameLogic.LanguagePath + SettingsManager.UserSettings["Language"] + ".txt";
        if (!File.Exists(_language))
            _language = GameLogic.LanguagePath + "ru.txt";

        using (StreamReader sr = new StreamReader(_language))
        {
            string line;
            string[] splitLine = new string[2];
            while ((line = sr.ReadLine()) != null)
            {
                splitLine = line.Split('=');
                splitLine[0] = splitLine[0].Replace(" ", "");
                AddLanguageText(splitLine[0], splitLine[1]);
            }
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
