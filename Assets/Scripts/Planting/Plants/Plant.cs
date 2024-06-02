using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantSO plantSO;
    public PlotType plot;
    public bool isOnCorrectPlot;

    public Transform targetTransform;

    private void Start()
    {
        float spawnRate = plantSO.baseSpawnRate;

        if(isOnCorrectPlot == false)
        {
            spawnRate *= 2;
        }

        InvokeRepeating(nameof(TrySpawnInsect), Random.Range(3, spawnRate), spawnRate);

        if(targetTransform == null)
        {
            targetTransform = transform;
        }

        StartCoroutine(DecayTimer());
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
    IEnumerator DecayTimer()
{
    float startTime = Time.realtimeSinceStartup;
    float elapsedTime = 0f;

    while (elapsedTime < plantSO.decayTime)
    {
        yield return null; // Wait for the next frame
        elapsedTime = Time.realtimeSinceStartup - startTime;
    }

    Destroy(gameObject); // Destroy the plant object after decay time has elapsed
}

}
