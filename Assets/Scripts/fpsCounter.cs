using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fpsCounter : MonoBehaviour {

    private Text fps;

    private void Start()
    {
        fps = GetComponent<Text>();
    }

    private void Update()
    {
        float _fps = 1.0f / Time.deltaTime;
        fps.text = _fps.ToString("##");
    }
}
