using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
public class PlantSO : ScriptableObject
{
    public string id;
    public string plantName;
    [TextArea(10, 20)]
    public string description;
    public bool unlocked;

    public Transform prefab;
    public Transform visual;

    public Sprite sprite;

    public PlotType[] correctPlots;
    public GameObject[] insects;
    public float baseSpawnRate;
}
