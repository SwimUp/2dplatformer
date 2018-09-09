using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public sealed class InputManager : MonoBehaviour {

    public static InputManager instance;

    public static readonly Dictionary<string, KeyCode> KeyBindings = new Dictionary<string, KeyCode>();
    private string[] _basicKeys = new string[]
    {
        "RightKey = D",
        "LeftKey = A",
        "JumpKey = Space",
        "CrouchKey = C",
        "AttackKey = Mouse0",
        "UseKey = E"
    };

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

        string _keysPath = GameLogic.KeysPath;

        if (!File.Exists(_keysPath))
        {
            File.Create(_keysPath).Close();
            int i;
            using (StreamWriter sw = new StreamWriter(_keysPath, false, System.Text.Encoding.Default))
            {
                for(i = 0; i < instance._basicKeys.Length; i++)
                {
                    sw.WriteLine(instance._basicKeys[i]);
                }
            }
        }

        using (StreamReader sr = new StreamReader(_keysPath, System.Text.Encoding.Default))
        {
            string line;
            string[] splitLine = new string[2];
            while ((line = sr.ReadLine()) != null)
            {
                line = line.Replace(" ", "");
                splitLine = line.Split('=');
                foreach (KeyCode KCode in Enum.GetValues(typeof(KeyCode)))
                    if (KCode.ToString() == splitLine[1])
                        AddNewKey(splitLine[0], KCode);
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
}
