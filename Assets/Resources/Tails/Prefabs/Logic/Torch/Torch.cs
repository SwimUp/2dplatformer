using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : InteractObject
{
    private bool _toggleFire = true;
    public GameObject Light;

    public override void InitObject()
    {
        base.InitObject();

        _toggleFire = true;
    }
    public override void onUse()
    {
        AnimateObject.SetActive(!AnimateObject.activeSelf);
        Light.SetActive(!Light.activeSelf);
        _toggleFire = !_toggleFire;
    }
}
