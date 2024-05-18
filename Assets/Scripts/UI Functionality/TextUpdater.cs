using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text descriptionText;
    [SerializeField]
    private PlantSO connectedPlant;

    void Start()
    {
        PlantingManager.instance.onPlantUnlocked += OnUnlock;
    }

    private void OnUnlock(PlantSO plant)
    {
        if(connectedPlant == plant)
        {
            nameText.text = plant.itemName;
            descriptionText.text = plant.description;

            //image = plant.sprite;
        }
    }
}
