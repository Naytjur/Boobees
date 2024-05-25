using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public LocalizedString message; // Scan pop up message that lets you know what you've scannedS
    private LocalizeStringEvent localizeStringEvent; // the event that where the message will be inserted

    private void Start()
    {
        PlantingManager.instance.onPlantUnlocked += ShowScanPopUp;
    }
    private void ShowScanPopUp(PlantSO plant)
    {
        GameObject popUp = Instantiate(popUpPrefab, popUpCanvas.transform);
        ScanPopUpInfo scanPopUpInfo = popUp.GetComponent<ScanPopUpInfo>();
        //scanPopUpInfo.scanText.text = "You have unlocked " + plant.itemName + "!";
        scanPopUpInfo.flowerImage.sprite = plant.sprite;

        localizeStringEvent = popUp.GetComponent<ScanPopUpInfo>().localizeStringEvent;
        localizeStringEvent.StringReference = message;
    }


}
