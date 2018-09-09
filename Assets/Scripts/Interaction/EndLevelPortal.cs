using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelEnd : InteractObject {

    public override void onUse()
    {

        GameLogic.LoadLevel("Level0", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
