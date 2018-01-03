using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMove : MonoBehaviour {

    public float movementSpeed = 10.0f;

	// Use this for initialization
	void Start () {
        var camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A))
            transform.position -= Camera.main.transform.right * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            transform.position += Camera.main.transform.right * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            transform.position += Camera.main.transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            transform.position -= Camera.main.transform.forward * movementSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Space))
            transform.position += new Vector3(0, movementSpeed * Time.deltaTime, 0);

        if (Input.GetKey(KeyCode.E))
            transform.position -= new Vector3(0, movementSpeed * Time.deltaTime, 0);

    }
}
