using System.Linq;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public sealed class InputManager : MonoBehaviour {

    public static InputManager instance;

    public static readonly Dictionary<string, KeyCode> KeyBindings = new Dictionary<string, KeyCode>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    public static void LoadSettingsKeys()
    {
        if (GameLogic.isLoad)
            return;

        InputData inputData = new InputData();
        string _keysPath = GameLogic.KeysPath;

        if (!File.Exists(_keysPath))
        {
            inputData.SetDefault();
            Utility.SerializeData(_keysPath, inputData);
        }

        inputData = (InputData)Utility.DeserializeData(_keysPath, typeof(InputData));

        int i;
        for(i = 0; i < inputData.InputList.Count; i++)
        {
            SettingsData data = inputData.InputList[i];
            foreach (KeyCode KCode in Enum.GetValues(typeof(KeyCode)))
            {
                if(KCode.ToString() == data.Value)
                {
                    AddNewKey(data.Key, KCode);
                }
            }
        }
    }
    public static bool AddNewKey(string Action, KeyCode Key)
    {
        if (GameLogic.isLoad)
            return false;

        if (KeyBindings.ContainsKey(Action))
            return false;

        KeyBindings.Add(Action, Key);

        return true;
    }
    public static bool UpdateKey(string Action, KeyCode NewKey)
    {
        if (!KeyBindings.ContainsKey(Action))
            return false;

        KeyBindings[Action] = NewKey;

        return true;
    }
    public static void SaveSettings()
    {
        InputData inputData = new InputData();

        foreach (KeyValuePair<string, KeyCode> d in KeyBindings)
        {
            SettingsData data = new SettingsData(d.Key, d.Value.ToString());
            inputData.InputList.Add(data);
        }

        Utility.SerializeData(GameLogic.KeysPath, inputData);
    }
}
[Serializable]
public sealed class InputData
{
    [System.Xml.Serialization.XmlArrayItem(ElementName = "Action")]
    public List<SettingsData> InputList = new List<SettingsData>();

    [NonSerialized]
    private readonly Dictionary<string, string> _defaultSettings = new Dictionary<string, string>
    {
        {"RightKey", "D" },
        {"LeftKey", "A" },
        {"JumpKey", "Space" },
        {"CrouchKey", "C" },
        {"AttackKey", "Mouse0" },
        {"UseKey", "E" }
    };

    public InputData()
    {
    }

    public void SetDefault()
    {
        foreach(KeyValuePair<string,string> d in _defaultSettings)
        {
            InputList.Add
                (
                    new SettingsData(d.Key, d.Value)
                );
        }
    }
}
