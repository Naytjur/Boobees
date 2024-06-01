using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildData
{
    public string buildingID;
    public int gridX;
    public int gridZ;
    public int buildingRotation;

    public List<PlantData> placedPlants = new List<PlantData>();

    public BuildData(string id, int gridX, int gridZ, int rot)
    {
        this.buildingID = id;
        this.buildingRotation = rot;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
}
