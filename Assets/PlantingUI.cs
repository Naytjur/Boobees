using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;

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
            Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
            button.GetComponent<SelectPlant>().SetIndex(index);
            button.GetComponentInChildren<TMP_Text>().text = plant.name;
            button.GetComponent<Button>().interactable = plant.unlocked;
            index++;
        }
    }
}
