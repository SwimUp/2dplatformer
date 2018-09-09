using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct PortalData
{
    public string ObjectName;
    public Status PortalStatus;

    public PortalData(string Name, Status _PortalStatus)
    {
        ObjectName = Name;
        PortalStatus = _PortalStatus;
    }
}
[Serializable]
public sealed class DataManager {
    /* Стадии:
     * 0 - начало,
     * 1 - добыт камень огня,
     * 2 - добыт камень земли,
     * 3 - добыт камень воды,
     * 4 - добыт камень воздуха
    */
    public int Stage = 0;
    public List<PortalData> Destinations = new List<PortalData>();

    public void AddPortal(GameObject PortalObj)
    {
        string _name = PortalObj.name;
        Status _status = PortalObj.GetComponent<Portal>().DestinationStatus;
        PortalData _data = new PortalData(_name, _status);
        Destinations.Add(_data);
    }
    public PortalData GetPortalByName(string Name)
    {
        foreach(PortalData data in Destinations)
        {
            if (data.ObjectName == Name)
                return data;
        }

        return Destinations[0];
    }
    public void SetPortalData(PortalData portal)
    {
        int i;
        for(i = 0; i < Destinations.Count; i++)
        {
            if(Destinations[i].ObjectName == portal.ObjectName)
            {
                PortalData _data = Destinations[i];
                _data.PortalStatus = portal.PortalStatus;
                Destinations[i] = _data;
                break;
            }
        }
    }
}
