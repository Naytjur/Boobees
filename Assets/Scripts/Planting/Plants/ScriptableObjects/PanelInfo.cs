using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfo : MonoBehaviour
{
    public List<PlantSO> plantInfo = new List<PlantSO>();

    private string IdNum;
    private int index;
    private string PlantName;
    public Text canvas;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < plantInfo.Count; i++)
        {
            if (plantInfo[i].id == IdNum)
            {
                index = i;
            }
        }


    }

    // Update is called once per frame
    void Update()
    { 
        
    }

    public void ChangeID(string NumberToChangeTo)
    {
        IdNum = NumberToChangeTo;
    }


    public void ChangePlantName(string PlantNameToChange)
    {
        plantInfo[index].plantName = PlantNameToChange;
    }


}