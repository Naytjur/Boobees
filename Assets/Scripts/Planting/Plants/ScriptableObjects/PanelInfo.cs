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

    private string IdNum;
    private int index;
    public TextMeshProUGUI PlantName;
    public TextMeshProUGUI PlantDescription;
    public GameObject PlantModel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    public void ChangeID(string NumberToChangeTo)
    {
        IdNum = NumberToChangeTo;

        for (int i = 0; i < plantInfo.Count; i++)
        {
            if (plantInfo[i].id == IdNum)
            {
                index = i;
            }
        }
        
    }


    public void ChangePlantName()
    {
        PlantName.text = plantInfo[index].plantName;
        PlantDescription.text = plantInfo[index].description;
        Debug.Log(plantInfo.Count);
        Debug.Log(plantInfo[index].plantName);
        Debug.Log(plantInfo[index].description);
        Debug.Log(index);
    }


}