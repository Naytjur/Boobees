using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ItemDisplay : MonoBehaviour
{
    [HideInInspector]
    public ItemInfo item;
    [SerializeField]
    private Image displayImage;
    //[SerializeField]
    //private TMP_Text displayName;

    private ItemDisplayButton button;

    public LocalizeStringEvent plantNameEvent;
    public LocalizedString plantName;
    public LocalizedString plantNotDiscoveredMessage;

    public void UpdateDisplay(ItemInfo item)
    {
        if(button == null)
        {
            if(GetComponent<ItemDisplayButton>())
            {
                button = GetComponent<ItemDisplayButton>();
            }
        }
        if(button != null) 
        {
            button.UpdateButton(item);
        }

        this.gameObject.SetActive(true);
        this.item = item;
        displayImage.sprite = item.sprite;
        //displayName.text = item.itemName;

        plantName = item.itemNameLocalizedString;
        plantNameEvent.StringReference = plantName;

        if (!item.unlocked)
        {
            //displayName.text = "not discovered";
           plantNameEvent.StringReference = plantNotDiscoveredMessage;
        }
    }

    public void HideDisplay()
    {
        this.gameObject.SetActive(false);
    }
}
