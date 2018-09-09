using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Status : int
{
    ACTIVE = 0,
    COMPLETE,
    DISABLE
}
[RequireComponent(typeof(Portal))]
public sealed class Portal : InteractObject {

    [SerializeField]
    internal Status DestinationStatus = Status.ACTIVE;
    [SerializeField]
    private string _destinationLevel = null;
    public string DestinationLevel
    {
        get { return _destinationLevel; }
    }

    public override void InitObject()
    {
        base.InitObject();
    }
    
    public override void onUse()
    {
        if (DestinationStatus == Status.COMPLETE)
            return;

        if (DestinationLevel != null)
        {
            GameLogic.CurrentPortal = this;
            GameLogic.LoadLevel(DestinationLevel, UnityEngine.SceneManagement.LoadSceneMode.Single);
        }
    }
}
