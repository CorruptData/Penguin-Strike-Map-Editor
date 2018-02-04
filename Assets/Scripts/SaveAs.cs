using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveAs : MonoBehaviour {

    public Text nameBox;
    public GameObject SaveBox;
    private bool saveBoxState = false;

    private void Start()
    {
        SaveBox.SetActive(false);
    }

    public void ToggleSaveBox()
    {
        saveBoxState = !saveBoxState;
        SaveBox.SetActive(saveBoxState);
    }

    public void Save()
    {
        Camera.main.GetComponent<BlockPlacer>().SaveData(nameBox.text);
    }
}
