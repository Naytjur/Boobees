using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantData
{
    public string plantID;
    public float worldX;
    public float worldY;
    public float worldZ;
    public int plantRotation; 

    public PlantData(string id, float worldX, float worldZ, int rot)
    {
        this.plantID = id;
        this.plantRotation = rot;
        this.worldX = worldX;
        this.worldZ = worldZ;
    }
}
