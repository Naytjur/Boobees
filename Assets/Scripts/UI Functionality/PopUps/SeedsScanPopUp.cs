using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class SeedsScanPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpPrefab;
    [SerializeField]
    private GameObject popUpCanvas;
    public LocalizedString plantName;

    private LocalizeStringEvent nameLocalizeStringEvent;

    private void Start()
    {
        PlantingManager.instance.onSeedsGained += ShowSeedsScanPopUp;
    }
    private void ShowSeedsScanPopUp(PlantSO plant)
    {
        GameObject popUp = Instantiate(popUpPrefab, popUpCanvas.transform);
        ScanSeedsPopUpInfo scanSeedsPopUpInfo = popUp.GetComponent<ScanSeedsPopUpInfo>();
        plantName = plant.itemNameLocalizedString;
        scanSeedsPopUpInfo.addedSeedAmount = PlantingManager.instance.seedsGained;
        nameLocalizeStringEvent = scanSeedsPopUpInfo.nameLocalizeStringEvent;
        nameLocalizeStringEvent.StringReference = plantName;
        scanSeedsPopUpInfo.flowerImage.sprite = plant.sprite;
        scanSeedsPopUpInfo.seedsLocalizeStringEvent.RefreshString();
    }

}
