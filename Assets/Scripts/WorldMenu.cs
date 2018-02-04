using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class WorldMenu : MonoBehaviour {

    //OG button for copying
    public GameObject Button_Template;

    GameObject worldMenu;
    bool worldMenuState = false;

    // Use this for initialization
    void Start () {

        worldMenu = this.gameObject;
        worldMenu.SetActive(false);

        foreach (string file in System.IO.Directory.GetFiles("Saves"))
        {
            string ext = Path.GetExtension(file);
            if (ext.ToLower() == ".pscw")
            {
                //Get the name from the file
                string name = Path.GetFileNameWithoutExtension(file);

                //Create a clone of the original button
                GameObject go = Instantiate(Button_Template) as GameObject;
                go.SetActive(true);
                WorldListButton WLB = go.GetComponent<WorldListButton>();
                WLB.SetName(name);
                go.transform.SetParent(Button_Template.transform.parent);
                go.transform.localScale = new Vector3 (1, 1, 1);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Toggle the menu
    public void Toggle ()
    {
        worldMenuState = !worldMenuState;
        worldMenu.SetActive(worldMenuState);
    }

    public void ButtonClicked(string str)
    {
        //Write the file to a temporary location to load
        FileUtil.ReplaceFile("saves/" + str + ".pscw", "saves/temp");
        //Load new scene
        SceneManager.LoadScene("MapEditorArea", LoadSceneMode.Single);
    }
}
