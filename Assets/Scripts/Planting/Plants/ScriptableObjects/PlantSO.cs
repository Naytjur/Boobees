using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
[System.Serializable]
public class PlantSO : ItemInfo
{
    public PlotType[] correctPlots;
    public GameObject[] insects;
    public float baseSpawnRate;
}
