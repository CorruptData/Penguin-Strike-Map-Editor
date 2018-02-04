using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.IO;

public class MenuAction : MonoBehaviour
{
    void Start()
    {

    }

    public void LoadNewMapScene()
    {
        string[] e = {"{}"};
        File.WriteAllLines("saves/temp", e);

        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("MapEditorArea", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
