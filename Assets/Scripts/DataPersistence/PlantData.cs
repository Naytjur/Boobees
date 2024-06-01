using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlantData
{
    public string plantID;
    public float x;
    public float y;
    public float z;
    public float timer;

    public PlantData(string id, float x, float y, float z, float timer)
    {
        this.plantID = id;
        this.x = x;
        this.y = y;
        this.z = z;
        this.timer = timer;
    }
}
