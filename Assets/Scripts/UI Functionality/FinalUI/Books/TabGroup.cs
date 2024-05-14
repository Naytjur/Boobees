using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<TabButton> buttons;
    [SerializeField]
    private List<GameObject> tabObjects;

    private TabButton selectedTab;

    public void Subscribe(TabButton button)
    {
        if (buttons == null)
        {
            buttons = new List<TabButton>();
        }

        buttons.Add(button);
    }

    public void OnTabSelected(TabButton button)
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
