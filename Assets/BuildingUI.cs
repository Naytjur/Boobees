using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField]
    private Transform buttonPrefab;
    [SerializeField]
    private Transform buttonContainerTransform;

    

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
        }

        foreach (BuildingSO building in BuildManager.instance.GetBuildingList())
        {
            Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
            button.GetComponent<SelectBuilding>().SetIndex(index);
            button.GetComponentInChildren<TMP_Text>().text = building.buildingName;
            index++;
        }
    }
}
