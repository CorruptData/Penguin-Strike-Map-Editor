using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGround : MonoBehaviour {

    private float sizeX = 2.2f;
    private float sizeZ = 2.2f;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

	}

    //Set the width (East-West)
    public void ResizeWidth (string s)
    {
        int x = 0;
        bool a = int.TryParse(s, out x);

        if (x >= 20 && a)
        {
            sizeX = x / 10.0f * 1.1f;
            transform.localScale = new Vector3(sizeX, 1, sizeZ);
        }
    }

    //Set the height (North-South)
    public void ResizeHeight (string s)
    {
        int z = 0;
        bool a = int.TryParse(s, out z);

        if (z >= 20 && a)
        {
            sizeZ = z / 10.0f * 1.1f;
            transform.localScale = new Vector3(sizeX, 1, sizeZ);
        }
    }

}
