using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldListButton : MonoBehaviour
{

    private string Name;
    public Text ButtonText;
    public WorldMenu WM;

    public void SetName(string name)
    {
        Name = name;
        ButtonText.text = name;
    }
    public void Button_Click()
    {
        WM.ButtonClicked(Name);

    }
}