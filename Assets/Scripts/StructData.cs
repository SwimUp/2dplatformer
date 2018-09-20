using System;
using System.Xml.Serialization;

[Serializable]
public struct PlayerInfo
{
    public PlayerStatus PlayerStatus;
    public string PlayerName;
    public string PathData;

    public PlayerInfo(PlayerStatus Status, string Name, string Path)
    {
        this.PlayerStatus = Status;
        PlayerName = Name;
        PathData = Path;
    }
}
[Serializable]
public struct SettingsData
{
    public string Key;
    public string Value;

    public SettingsData(string Key, string Value)
    {
        this.Key = Key;
        this.Value = Value;
    }
}
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
public struct PlayerData
{

}
