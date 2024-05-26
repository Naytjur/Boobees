using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;

    private List<Transform> buildButtons = new List<Transform>();

    public LocalizeStringEvent buildingNameEvent;
    public LocalizedString buildingName;
    public LocalizeStringEvent buildingCostEvent;
    public LocalizedString buildCostFree;
    public LocalizeStringEvent buildingUnlockEvent;
    public LocalizedString buildUnlocklevel;

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
            select.buildingCost = building.cost;
            select.buildingUnlockLevel = building.unlockLevel;
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

            buildingName = select.building.itemNameLocalizedString;
            buildingNameEvent = select.buildNameLocalizeStringEvent;
            buildingNameEvent.StringReference = buildingName;

            select.button.interactable = select.building.unlocked && select.building.HasCountLeft() && ScoreManager.instance.CanAfford(select.building.cost);

            select.buildCostLocalizationEvent.RefreshString();
            select.buildUnlockLocalizationEvent.RefreshString();

            if (select.building.cost == 0)
            {
                buildingCostEvent = select.buildCostLocalizationEvent;
                buildingCostEvent.StringReference = buildCostFree;
            }
            if (select.building.unlocked)
            {
                select.buildingAmountText.text = select.building.count.ToString() + "/" + select.building.maxCount.ToString();
            }
        }
    }

    private void UpdateBuildButtons()
    {
        foreach (Transform button in buildButtons)
        {
            SelectBuilding select = button.GetComponent<SelectBuilding>();

            buildingName = select.building.itemNameLocalizedString;
            buildingNameEvent = select.buildNameLocalizeStringEvent;
            buildingNameEvent.StringReference = buildingName;

            select.button.interactable = select.building.unlocked && select.building.HasCountLeft() && ScoreManager.instance.CanAfford(select.building.cost);

            select.buildCostLocalizationEvent.RefreshString();
            select.buildUnlockLocalizationEvent.RefreshString();

            if (select.building.cost == 0)
            {
                buildingCostEvent = select.buildCostLocalizationEvent;
                buildingCostEvent.StringReference = buildCostFree;
            }
            if (select.building.unlocked)
            {
                select.buildingAmountText.text = select.building.count.ToString() + "/" + select.building.maxCount.ToString();
            }
        }
    }
}
