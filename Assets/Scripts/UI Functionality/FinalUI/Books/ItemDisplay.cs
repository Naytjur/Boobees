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
    [SerializeField]
    private Button button;
    [SerializeField]
    private UICatalogue catalogue;

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
        button.interactable = item.unlocked;
        
        button.onClick.AddListener(OpenPage);
    }

    public void HideDisplay()
    {
        this.gameObject.SetActive(false);
    }

    private void OpenPage()
    {
        if (item is CatalogueItemInfo catalogueItem)
        {
            catalogue.OpenInfoPage(catalogueItem);
        }
    }
}
