using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ScanPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpPrefab;
    [SerializeField]
    private GameObject popUpCanvas;
    public LocalizedString plantName;

    private LocalizeStringEvent localizeStringEvent;

    private void Start()
    {
        PlantingManager.instance.onPlantUnlocked += ShowScanPopUp;
    }
    private void ShowScanPopUp(PlantSO plant)
    {
        GameObject popUp = Instantiate(popUpPrefab, popUpCanvas.transform);
        ScanPopUpInfo scanPopUpInfo = popUp.GetComponent<ScanPopUpInfo>();
        //scanPopUpInfo.scanText.text = "You have unlocked " + plant.itemName + "!";
        plantName = plant.itemNameStringEvent.StringReference;
        localizeStringEvent = popUp.GetComponent<ScanPopUpInfo>().localizeStringEvent;
        localizeStringEvent.StringReference = plantName;
        scanPopUpInfo.flowerImage.sprite = plant.sprite;
        //scanPopUpInfo.unlockedPlant = plantName;
    }


}
