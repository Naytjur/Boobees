using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TabGroup : MonoBehaviour
{
    private List<UITabButton> buttons;
    [SerializeField]
    private List<GameObject> tabObjects;

    private UITabButton selectedTab;

    public void Subscribe(UITabButton button)
    {
        if (buttons == null)
        {
            buttons = new List<UITabButton>();
        }

        buttons.Add(button);
    }

    public void OnTabSelected(UITabButton button)
    {
        selectedTab = button;

        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < tabObjects.Count; i++) 
        {
            if(i == index)
            {
                tabObjects[i].SetActive(true);
            }
            else
            {
                tabObjects[i].SetActive(false);
            }
        }
    }
}
