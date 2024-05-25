using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildData
{
    public string buildingID;
    public int buildingRotation; 

    public BuildData(string id, int rot)
    {
        this.buildingID = id;
        this.buildingRotation = rot;
    }
}
