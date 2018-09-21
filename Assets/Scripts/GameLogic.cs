using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

public sealed class GameLogic : MonoBehaviour {

    public static GameLogic instance = null;
    [SerializeField]
    private Canvas GameUICanvas;
    [SerializeField]
    private GameObject EscapeMenu;

    public static readonly string LanguagePath = @"Resources/Language/";
    public static readonly string SettingsPath = @"GameSettings.xml";
    public static readonly string KeysPath = @"PlayerBinds.ini";
    public static readonly string DataPath = @"Data/";
    //public static readonly string DataPath = @"2d platformer_Data/Data/playerdata.dat";

    /* Была ли первичная загрузка данных */
    public static bool isLoad = false;
    /* Начата ли игра? */
    public static bool StartGame = false;

    /* Текущий портал игрока */
    public static Portal CurrentPortal = null;

    /* Делегат для событий загрузки 
     * Порядок событий:
     * - onFirstLoad - первичная загрузка игры (1 раз)
     * - onGameInit - вторичная загрузка игры (1 раз)
     * - onLevelLoad - загрузка уровня (каждый раз)
     */
    public delegate void LoadEventsHandler();
    public static LoadEventsHandler onFirstLoad;
    public static LoadEventsHandler onGameInit;
    public static LoadEventsHandler onLevelLoad;

    /* Текущий загруженный игрок */
    public static PlayerInfo cPlayerInfo;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        if (!isLoad)
        {
            SettingsManager.LoadSettings();
            InputManager.LoadSettingsKeys();
            LanguageManager.LoadLanguage();

            isLoad = true;

            onFirstLoad?.Invoke();
        }
    }
	private void Start () {

        DontDestroyOnLoad(this);

        EscapeMenu.SetActive(false);

        onGameInit?.Invoke();

        SceneManager.sceneLoaded += OnSceneLoad;

    }
    /* Событие загрузки сцены */
    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        GameUICanvas.worldCamera = Camera.main;

        onLevelLoad?.Invoke();
    }
    private void Update()
    {
        if(StartGame && Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu.SetActive(!EscapeMenu.activeSelf);
        }
    }
    /* Загрузка сцен
     * @arg1 Level string/int - имя или цифра уровня
     * @arg2 Mode:
     * LoadSceneMode.Single
     * LoadSceneMode.Additive;
    */
    public static void LoadLevel(int Level, LoadSceneMode mode)
    {
        SceneManager.LoadScene(Level, mode);
    }
    public static void LoadLevel(string Level, LoadSceneMode mode)
    {
        SceneManager.LoadScene(Level, mode);
    }
    public void StartNewGame()
    {
        StartGame = true;

        LoadLevel(1, LoadSceneMode.Single);
    }
    public void LoadGame()
    {
        StartGame = true;
        LoadLevel(1, LoadSceneMode.Single);
    }
    /* Escape menu */
    public void ResumeGame()
    {
        EscapeMenu.SetActive(false);
    }
    public void ExitGame()
    {
        if (ElementsAltar.Data != null)
            Utility.SerializeData(cPlayerInfo.PathData, ElementsAltar.Data);

        Application.Quit();
    }
}
