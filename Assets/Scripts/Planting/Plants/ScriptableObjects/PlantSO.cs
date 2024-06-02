using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
[System.Serializable]
public class PlantSO : CatalogueItemInfo
{
    public PlotType[] correctPlots;
    public float baseSpawnRate;
    public int seedAmount;
    public int decayTime;
}
