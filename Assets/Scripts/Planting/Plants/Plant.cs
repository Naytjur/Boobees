using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public List<GameObject> insects = new List<GameObject>();
    public PlantSO plantSO;
    public PlotType plot;
    public bool isOnCorrectPlot;
    public float baseSpawnRate = 10f;

    public Transform targetTransform;

    private void Start()
    {
        float spawnRate = baseSpawnRate;

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
        foreach (GameObject insectPrefab in insects)
        {
            Insect insectScript = insectPrefab.GetComponent<Insect>();

            if (insectScript != null && Random.Range(1, 101) <= insectScript.GetRarityPercentage())
            {
                SpawnInsect(insectPrefab);
            }
        }
    }
    
    private void SpawnInsect(GameObject prefab)
    {
        Vector3 spawnPosition = targetTransform.position + Vector3.up * Random.Range(1f, 3f) + Random.insideUnitSphere * 2f;
        Insect newInsect = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<Insect>();

        newInsect.Spawn(targetTransform.position);
    }
}
