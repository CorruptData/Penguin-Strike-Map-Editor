using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour {

    void Start()
    {

    }

    //This area is for testing things
    //Pay no mind
    void Update()
    {
        //Toggle the fill tool
        //TODO: Move to a script on the toggle box
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject toggleObj = GameObject.Find("Toggle");
            var tick = toggleObj.GetComponent<Toggle>();
            tick.isOn = !tick.isOn;
        }
    }
}