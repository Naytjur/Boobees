using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

[RequireComponent(typeof(ItemDisplay))]
public class ItemDisplayButton : MonoBehaviour
{
    private ItemDisplay display;
    private Button button;

    [SerializeField]
    private UICatalogue catalogue;

    private void Start()
    {
        button = GetComponent<Button>();
        display = GetComponent<ItemDisplay>();
        display.onItemUpdate += UpdateButton;
    }

    private void UpdateButton(ItemInfo item)
    {
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
