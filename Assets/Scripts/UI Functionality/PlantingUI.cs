using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlantingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;

    public LocalizeStringEvent plantNameEvent;
    public LocalizedString plantName;
    public LocalizedString plantNotDiscoveredMessage;

    private void Start()
    {
        PlantingManager.instance.onPlantUnlocked += UpdatePlantButtons;
    }

    private void OnEnable()
    {
        LoadPlantButtons();
    }

    private void LoadPlantButtons()
    {
        int index = 0;

        foreach (Transform child in buttonContainerTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (PlantSO plant in PlantingManager.instance.GetPlantList())
        {
            if (plant.unlocked)
            {
                Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
                SelectPlant buttonInfo = button.GetComponent<SelectPlant>();
                buttonInfo.Setup();
                buttonInfo.SetIndex(PlantingManager.instance.GetPlantList().IndexOf(plant));
                //buttonInfo.text.text = plant.name;

                plantName = plant.itemNameLocalizedString;
                plantNameEvent = buttonInfo.plantNameLocalizeStringEvent;
                plantNameEvent.StringReference = plantName;

                buttonInfo.image.sprite = plant.sprite;
                button.GetComponent<Button>().interactable = plant.unlocked;
                index++;
            }
        }
        foreach (PlantSO plant in PlantingManager.instance.GetPlantList())
        {
            if (!plant.unlocked)
            {
                Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
                SelectPlant buttonInfo = button.GetComponent<SelectPlant>();
                buttonInfo.SetIndex(PlantingManager.instance.GetPlantList().IndexOf(plant));
                //buttonInfo.text.text = plant.name;

                /*plantName = plant.itemNameStringEvent.StringReference;
                plantNameEvent = buttonInfo.plantNameLocalizeStringEvent;
                plantNameEvent.StringReference = plantName;*/

                plantNameEvent = buttonInfo.plantNameLocalizeStringEvent;
                plantNameEvent.StringReference = plantNotDiscoveredMessage;


                buttonInfo.image.sprite = plant.sprite;
                button.GetComponent<Button>().interactable = plant.unlocked;
                index++;
            }
        }
    }

    private void UpdatePlantButtons(PlantSO unlockedPlant)
    {
        int index = 0;

        foreach (Transform button in buttonContainerTransform)
        {
            var plant = PlantingManager.instance.GetPlantByIndex(button.GetComponent<SelectPlant>().GetIndex());
            button.GetComponent<Button>().interactable = plant.unlocked;
            index++;
        }
    }
}
