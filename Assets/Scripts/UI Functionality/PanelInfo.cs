using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfo : MonoBehaviour
{
    public List<PlantSO> plantInfo = new List<PlantSO>();

    private int idNum;
    private int index;
    public TextMeshProUGUI plantName;
    public TextMeshProUGUI plantDescription;
    public Image plantModel;

    public void ChangeID(int NumberToChangeTo)
    {
        idNum = NumberToChangeTo;

        for (int i = 0; i < plantInfo.Count; i++)
        {
            if (plantInfo[i].id == idNum)
            {
                index = i;
            }
        }  
    }

    public void ChangePlantName()
    {
        plantName.text = plantInfo[index].itemName;
        plantDescription.text = plantInfo[index].description;
        Debug.Log(plantInfo.Count);
        Debug.Log(plantInfo[index].itemName);
        Debug.Log(plantInfo[index].description);
        Debug.Log(index);
    }


}