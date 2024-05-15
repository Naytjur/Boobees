using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
[System.Serializable]
public class PlantSO : ScriptableObject
{
    public string id;
    public string plantName;
    public string description;
    public bool unlocked;

    public Transform prefab;
    public Transform visual;

    public Sprite sprite;

    public PlotType[] correctPlots;
}
