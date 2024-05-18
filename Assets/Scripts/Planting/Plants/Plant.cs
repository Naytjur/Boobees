using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantSO plantSO;
    [HideInInspector]
    public PlotType plot;
    [HideInInspector]
    public bool isOnCorrectPlot;

    public Transform targetTransform;

    private void Start()
    {
        float spawnRate = plantSO.baseSpawnRate;

        if(isOnCorrectPlot == false)
        {
            spawnRate *= 2;
        }

        InvokeRepeating(nameof(TrySpawnInsect), spawnRate, spawnRate);

        if(targetTransform == null)
        {
            targetTransform = transform;
        }
    }

    public void AssignPlot(PlotType plotType)
    {
        plot = plotType;

        foreach(PlotType type in plantSO.correctPlots)
        {
            if(type == plot)
            {
                isOnCorrectPlot= true;
            }
        }
    }

    private void TrySpawnInsect()
    {
        foreach (ItemInfo item in plantSO.attractions)
        {
            if (item is InsectSO insect)
            { 
                if ( Random.Range(1, 101) <= insect.GetRarityPercentage())
                {
                    SpawnInsect(item.gardenPrefab);
                }
            }
        }
    }
    
    private void SpawnInsect(Transform prefab)
    {
        Vector3 spawnPosition = targetTransform.position + Vector3.up * Random.Range(1f, 3f) + Random.insideUnitSphere * 2f;
        Insect newInsect = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<Insect>();

        newInsect.Spawn(targetTransform.position);
    }
}
