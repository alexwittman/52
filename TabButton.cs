using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour {

    public GameObject Click, Menu;
    public Sprite Inactive, Active;
    public List<Button> OtherButtons;

    public void OpenMenu()
    {
        Menu.transform.SetAsLastSibling();
        Menu.transform.parent.transform.SetAsLastSibling();
        GetComponent<Image>().sprite = Active;
        foreach( Button button in OtherButtons)
        {
            button.GetComponent<Image>().sprite = Inactive;
        }
        Click.GetComponent<Button>().interactable = false;
    }

    public void CloseMenu()
    {
        foreach (Button button in OtherButtons)
        {
            button.GetComponent<Image>().sprite = Inactive;
        }
        Click.GetComponent<Button>().interactable = true;
    }

}
