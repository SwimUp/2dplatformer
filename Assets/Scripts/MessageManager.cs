using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class MessageManager : MonoBehaviour{

    public static MessageManager instance = null;

    private bool _isInit = false;
    private static GameObject _gameUI;
    public static GameObject GameUI
    {
        get
        {
            return _gameUI;
        }
    }

    [SerializeField]
    private GameObject _interactMessageUI;
    [SerializeField]
    private GameObject _systemMessageUI;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }
    private void Start()
    {
        _gameUI = GameObject.FindGameObjectWithTag("GameUI");
    }
    /* Сообщение при взаимодействии
     * @arg1 - true/false - показать/скрыть
    */ 
    public static void ShowInteractMessage(bool State)
    {
        Vector3 playerPos = Player.instance.gameObject.transform.position;
        Vector3 newPos = new Vector3(
            playerPos.x,
            playerPos.y + 0.23f,
            playerPos.z
        );

        instance._interactMessageUI.transform.position = newPos;
        instance._interactMessageUI.SetActive(State);
    }
}
