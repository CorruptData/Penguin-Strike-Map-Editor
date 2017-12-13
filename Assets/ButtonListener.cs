using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject toggleObj = GameObject.Find("Toggle");
            var tick = toggleObj.GetComponent<Toggle>();
            tick.isOn = !tick.isOn;
        }
    }
}