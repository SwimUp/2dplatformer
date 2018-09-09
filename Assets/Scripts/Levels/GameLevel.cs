using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevel : MonoBehaviour {

    [SerializeField]
    protected string _levelName;
    public string LevelName
    {
        get { return _levelName; }
    }
    protected static readonly List<string> InitLevels = new List<string>();

    private void Start()
    {
        SceneManager.sceneUnloaded += OnLevelUnload;

        if(!InitLevels.Contains(_levelName))
        {
            InitLevel();
            InitLevels.Add(_levelName);
        }

        LoadLevel();
    }
    public virtual void OnLevelUnload(Scene current)
    {

    }
    public virtual void InitLevel()
    {

    }
    public virtual void LoadLevel()
    {

    }
}
