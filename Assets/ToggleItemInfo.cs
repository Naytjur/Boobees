using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleItemInfo : MonoBehaviour
{
    private SelectBuilding selector;
    private Button button;
    private ItemInfoDisplay infoDisplay;

    private void Awake()
    {
        selector = GetComponentInParent<SelectBuilding>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(DisplayInfo);
    }

    public void Setup(SelectBuilding button, ItemInfoDisplay display)
    {
        selector = button;
        infoDisplay = display;
    }

    private void DisplayInfo()
    {
        infoDisplay.gameObject.SetActive(true);
        infoDisplay.UpdateText(selector.building.itemDescriptionLocalizedString);
    }

}
