using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeGround : MonoBehaviour {

    private float sizeX = 11f;
    private float sizeZ = 11f;

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

        if (x >= 10 && a)
        {
            sizeX = x / 10.0f;
            transform.position = new Vector3((sizeX * 5)-0.5f, -0.5f, (sizeZ * 5)-0.5f);
            transform.localScale = new Vector3(sizeX, 1, sizeZ);

            this.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(sizeX * 10, sizeZ * 10);
        }
    }

    //Set the height (North-South)
    public void ResizeHeight (string s)
    {
        int z = 0;
        bool a = int.TryParse(s, out z);

        if (z >= 10 && a)
        {
            sizeZ = z / 10.0f;
            transform.position = new Vector3((sizeX * 5) - 0.5f, -0.5f, (sizeZ * 5) - 0.5f);
            transform.localScale = new Vector3(sizeX, 1, sizeZ);

            this.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(sizeX * 10, sizeZ * 10);
        }
    }

}
