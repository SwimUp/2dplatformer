using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class ElementsAltar : GameLevel {

    public static DataManager Data;
    [SerializeField]
    private GameObject[] StagesObject;
    /* Текущий уровень */
    private GameObject _mainLevel;
    /* Текущие порталы */
    public static GameObject PortalsObj;

    public override void InitLevel()
    {
        Data = GameLogic.LoadPlayerData();
        _mainLevel = GameObject.Find("Level1");

        if(Data.Destinations.Count == 0)
        {
            PortalsObj = Instantiate(StagesObject[0]);
            GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach(GameObject portal in portals)
            {
                Data.AddPortal(portal);
            }

            GameLogic.SavePlayerData(Data);
        }
        else
        {
            PortalsObj = Instantiate(StagesObject[Data.Stage]);
            UpdatePortalData();
        }
        DontDestroyOnLoad(PortalsObj);
    }
    public override void LoadLevel()
    {
        PortalsObj.SetActive(true);
        if(GameLogic.CurrentPortal != null)
        {
            Portal _portal = GameLogic.CurrentPortal;
            _portal.DestinationStatus = Status.COMPLETE;
            SetPortalData(_portal);
            GameLogic.CurrentPortal = null;

            GameLogic.SavePlayerData(Data);
        }
    }
    public override void OnLevelUnload(Scene current)
    {
        PortalsObj.SetActive(false);
    }
    private void UpdatePortalData()
    {
        GameObject[] portals = GameObject.FindGameObjectsWithTag("Portal");
        if (portals.Length == 0)
            return;

        foreach (GameObject portal in portals)
        {
            PortalData _portalData = Data.GetPortalByName(portal.name);
            Portal _portal = portal.GetComponent<Portal>();

            _portal.DestinationStatus = _portalData.PortalStatus;
        }
    }
    private void SetPortalData(Portal portal)
    {
        PortalData _portalData = Data.GetPortalByName(portal.gameObject.name);
        _portalData.PortalStatus = portal.DestinationStatus;
        Data.SetPortalData(_portalData);
    }
}
