using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PanelInfo : MonoBehaviour
{
    public List<PlantSO> plantInfo = new List<PlantSO>();

    private string IdNum;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < plantInfo.Count; i++)
        {
            if (plantInfo[i].id == IdNum)
            {
                return;
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
        print(IdNum);
    }


}
