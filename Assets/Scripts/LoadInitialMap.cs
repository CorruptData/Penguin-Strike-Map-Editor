using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInitialMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        BlockPlacer BP = Camera.main.transform.GetComponent<BlockPlacer>();
        BP.LoadData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
