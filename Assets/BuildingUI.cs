using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

        foreach(BuildingSO building in BuildManager.instance.GetBuildingList())
        {
            Transform button = Instantiate(buttonPrefab, buttonContainerTransform);
            button.GetComponent<SelectBuilding>().SetIndex(index);
            button.GetComponentInChildren<TMP_Text>().text = building.name;
            index++;
        }
    }
}
