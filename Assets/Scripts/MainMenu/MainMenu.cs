using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public sealed class MainMenu : MonoBehaviour {

    //0 - mainmenu
    //1 - settings
    public GameObject[] MainMenuUI;
    //Массив всего текста в меню
    public Text[] MainMenuText;
    //кнопки
    public Dropdown[] SettingsDrop;
    //префаб кнопки
    public GameObject ButtonKey;

    private void Awake()
    {
        GameLogic.onGameInit += onGameInit;
    }

    private void onGameInit()
    {
        GameObject KeysMenu = GameObject.Find("ContentKeysData");

        /* Создаем кнопки для бинда кнопок */
        foreach(KeyValuePair<string, KeyCode> k in InputManager.KeyBindings)
        {
            GameObject _button = Instantiate(ButtonKey, KeysMenu.transform);
            Text name = _button.transform.Find("Name").gameObject.GetComponent<Text>();
            Text key = _button.transform.Find("Key").gameObject.GetComponent<Text>();

            _button.name = k.Key;
            name.text = LanguageManager.TextList[k.Key];
            key.text = k.Value.ToString();
        }
        /* =========================================================== */

        /* Перебираем массив объектов  и заменяем текст в соотв. с именем (имя - ключ) */
        int i;
        for (i = 0; i < MainMenuText.Length; i++)
        {
            MainMenuText[i].text = LanguageManager.TextList[MainMenuText[i].name];
        }
        /* ============================================== */

        LoadMenu(0);

        SettingsDrop[0].value = PlayerPrefs.GetInt("ResolutionID", 0);
        SettingsDrop[1].value = PlayerPrefs.GetInt("LanguageID", 0);

        SetResolution();
    }
    public void LoadMenu(int Menu)
    {
        int i;
        for (i = 0; i < MainMenuUI.Length; i++)
        {
            MainMenuUI[i].SetActive(false);
        }

        MainMenuUI[Menu].SetActive(true);
    }
    public void SaveSettings()
    {
        string path = GameLogic.SettingsPath;
        string path2 = GameLogic.KeysPath;
        string Resolution = SettingsManager.UserSettings["Resolution"];
        string Language = SettingsManager.UserSettings["Language"];
        GameObject KeysMenu = GameObject.Find("ContentKeysData");

        SettingsManager.SaveSettings();
        InputManager.SaveSettings();

        SetResolution();

        LoadMenu(0);
    }
    public void GetResolution(Text drop)
    {
        SettingsManager.UserSettings["Resolution"] = drop.text;
        PlayerPrefs.SetInt("ResolutionID", SettingsDrop[0].value);
    }
    public void GetLanguage(Text drop)
    {
        SettingsManager.UserSettings["Language"] = drop.text;
        PlayerPrefs.SetInt("LanguageID", SettingsDrop[1].value);
    }
    private void SetResolution()
    {
        string[] Resolution = SettingsManager.UserSettings["Resolution"].Split('x');
        int width = int.Parse(Resolution[0]);
        int height = int.Parse(Resolution[1]);
        Screen.SetResolution(width, height, true);
    }
    public void CreateNewProfile(InputField input)
    {
        if (input.text.Length <= 0)
            return;

        string FullplayerPath = string.Concat(GameLogic.DataPath, input.text);
        DirectoryInfo dirInfo = new DirectoryInfo(FullplayerPath);
        if(dirInfo.Exists)
        {
            return;
        }
        else
        {
            dirInfo.Create();
        }
        string playerPath = string.Concat(dirInfo.FullName, "\\", input.text, ".xml");
        File.Create(playerPath).Close();

        PlayerInfo pInfo = new PlayerInfo(PlayerStatus.NONSPAWNED, input.text, playerPath);
        GameLogic.cPlayerInfo = pInfo;

        Debug.Log(GameLogic.cPlayerInfo.PathData);

        DataManager _data = new DataManager(0);
        Utility.SerializeData(playerPath, _data);

        GameLogic.instance.StartNewGame();
    }
    public void Exit()
    {
        Application.Quit();
    }
}