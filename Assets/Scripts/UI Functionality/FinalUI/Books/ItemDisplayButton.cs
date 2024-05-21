using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(ItemDisplay))]
public class ItemDisplayButton : MonoBehaviour
{
    private ItemDisplay display;
    private Button button;

    [SerializeField]
    private UICatalogue catalogue;


    public void UpdateButton(ItemInfo item)
    {
        if (display == null)
        {
            display = GetComponent<ItemDisplay>();
        }
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.interactable = item.unlocked;
        button.onClick.AddListener(delegate { OpenPage(item); }) ;
    }

    private void OpenPage(ItemInfo item)
    {
        if (item is CatalogueItemInfo catalogueItem)
        {
            catalogue.OpenInfoPage(catalogueItem);
        }
    }
}
