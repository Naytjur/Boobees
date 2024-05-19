using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ItemDisplay : MonoBehaviour
{
    [HideInInspector]
    public ItemInfo item;
    [SerializeField]
    private Image displayImage;
    [SerializeField]
    private TMP_Text displayName;

    public event Action<ItemInfo> onItemUpdate;

    public void UpdateDisplay(ItemInfo item)
    {
        this.gameObject.SetActive(true);
        this.item = item;
        displayImage.sprite = item.sprite;
        displayName.text = item.itemName;
        if(!item.unlocked)
        {
            displayName.text = "not discovered";
        }
        onItemUpdate?.Invoke(item);
    }

    public void HideDisplay()
    {
        this.gameObject.SetActive(false);
    }
}
