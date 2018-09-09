using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChangeKey : MonoBehaviour, IPointerClickHandler {

    private Text _text;
    private bool _isClick;

    private void Start()
    {
        _text = transform.Find("Key").GetComponent<Text>();
    }
    private void Update()
    {
        if (_isClick)
        {
            foreach (KeyCode KCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(KCode))
                {
                    _text.text = KCode.ToString();
                    InputManager.UpdateKey(this.name, KCode);
                    _isClick = false;
                }
            }
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!_isClick)
        {
            _isClick = true;
            _text.text = LanguageManager.TextList["PressKey"];
        }
    }
}
