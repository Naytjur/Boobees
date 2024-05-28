using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using static BuildManager;

public class BuildingUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;
    [SerializeField]
    private GameObject buttonContainer;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button rotateButton;
    [SerializeField]
    private Button removeButton;

    private List<Transform> buildButtons = new List<Transform>();

    //[Header("Localization")]
    private LocalizeStringEvent buildingNameEvent;
    private LocalizedString buildingName;
    private LocalizeStringEvent buildingCostEvent;
    private LocalizedString buildCostFree;
    private LocalizeStringEvent buildingUnlockEvent;
    private LocalizedString buildUnlocklevel;

    void Start()
    {
        ScoreManager.onLevelUp += UpdateBuildButtons;
        BuildManager.onStateChanged += UpdateOverlay;
        BuildManager.onStateChanged += UpdateBuildButtons;

        rotateButton.onClick.RemoveAllListeners();
        removeButton.onClick.RemoveAllListeners();
        rotateButton.onClick.AddListener(BuildManager.instance.RotateBuilding);
        removeButton.onClick.AddListener(BuildManager.instance.RemoveBuilding);
    }

    private void OnEnable()
    {
        LoadBuildButtons();
    }

    private void UpdateOverlay(BuildManager.BuildState state)
    {
        confirmButton.gameObject.SetActive(state != BuildState.Unselected);
        rotateButton.gameObject.SetActive(state != BuildState.Unselected);
        buttonContainer.gameObject.SetActive(state == BuildState.Unselected);
        removeButton.gameObject.SetActive(state == BuildState.Moving);
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

    private void UpdateBuildButtons(BuildManager.BuildState state)
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
