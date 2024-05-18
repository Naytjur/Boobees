using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [HideInInspector]
    public ItemInfo item;
    [SerializeField]
    private Image displayImage;
    [SerializeField]
    private TMP_Text displayName;
    [SerializeField]
    private Button button;

    public void UpdateDisplay(ItemInfo item)
    {
        this.gameObject.SetActive(true);
        this.item = item;
        displayImage.sprite = item.sprite;
        displayName.text = item.itemName;
        button.interactable = item.unlocked;
    }

    public void HideDisplay()
    {
        this.gameObject.SetActive(false);
    }
}
