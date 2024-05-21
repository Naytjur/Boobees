using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;

    private List<Transform> buildButtons = new List<Transform>();

    void Start()
    {
        ScoreManager.onLevelUp += UpdateBuildButtons;
        BuildManager.onBuildingPlaced += UpdateBuildButtons;
    }

    private void OnEnable()
    {
        LoadBuildButtons();
    }

    private void LoadBuildButtons()
    {
        int index = 0;

        foreach (Transform child in buttonContainerTransform)
        {
            Destroy(child.gameObject);
            buildButtons.Clear();
        }

        foreach (BuildingSO building in BuildManager.instance.GetBuildingList())
        {
            Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
            SelectBuilding select = button.GetComponent<SelectBuilding>();
            select.SetIndex(index);
            select.building = building;
            index++;
            buildButtons.Add(button);
        }
        UpdateBuildButtons();
    }

    private void UpdateBuildButtons(int level)
    {
        foreach (Transform button in buildButtons)
        {
            SelectBuilding select = button.GetComponent<SelectBuilding>();
            select.buildingNameText.text = select.building.itemName;
            select.button.interactable = select.building.unlocked && select.building.HasCountLeft() && ScoreManager.instance.CanAfford(select.building.cost);
            select.buildingAmountText.text = select.building.count.ToString() + "/" + select.building.maxCount.ToString();
            select.buildingCostText.text = "Cost: " + select.building.cost.ToString();
            if(select.building.cost == 0)
            {
                select.buildingCostText.text = "Free!";
            }
        }
    }

    private void UpdateBuildButtons()
    {
        foreach (Transform button in buildButtons)
        {
            SelectBuilding select = button.GetComponent<SelectBuilding>();
            select.buildingNameText.text = select.building.itemName;
            select.button.interactable = select.building.unlocked && select.building.HasCountLeft() && ScoreManager.instance.CanAfford(select.building.cost);
            select.buildingAmountText.text = select.building.count.ToString() + "/" + select.building.maxCount.ToString();
            select.buildingCostText.text = "Cost: " + select.building.cost.ToString();
            if (select.building.cost == 0)
            {
                select.buildingCostText.text = "Free!";
            }
        }
    }
}
