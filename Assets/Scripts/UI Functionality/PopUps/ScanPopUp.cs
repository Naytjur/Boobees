using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScanPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpPrefab;
    [SerializeField]
    private GameObject popUpCanvas;

    private void Start()
    {
        PlantingManager.instance.onPlantUnlocked += ShowScanPopUp;
    }
    private void ShowScanPopUp(PlantSO plant)
    {
        GameObject popUp = Instantiate(popUpPrefab, popUpCanvas.transform);
        ScanPopUpInfo scanPopUpInfo = popUp.GetComponent<ScanPopUpInfo>();
        scanPopUpInfo.scanText.text = "You have unlocked " + plant.plantName + "!";
        scanPopUpInfo.flowerImage.sprite = plant.sprite;
    }
}
