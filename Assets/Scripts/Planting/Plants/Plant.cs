using System;
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
    private float elapsedTime = 0f;
    private float startTime;
    private Coroutine decayCoroutine;
    private Plot currentPlot;

    private void Start()
    {
        float spawnRate = plantSO.baseSpawnRate;

        if (isOnCorrectPlot == false)
        {
            spawnRate *= 2;
        }

        InvokeRepeating(nameof(TrySpawnInsect), UnityEngine.Random.Range(3, spawnRate), spawnRate);

        if (targetTransform == null)
        {
            targetTransform = transform;
        }
    }

    public void AssignPlot(PlotType plotType)
    {
        plot = plotType;

        foreach (PlotType type in plantSO.correctPlots)
        {
            if (type == plot)
            {
                isOnCorrectPlot = true;
            }
        }
    }

    public void SetPlot(Plot plot)
    {
        currentPlot = plot;
    }

    private void TrySpawnInsect()
    {
        foreach (ItemInfo item in plantSO.attractions)
        {
            if (item is InsectSO insect)
            {
                if (UnityEngine.Random.Range(1, 101) <= insect.GetRarityPercentage())
                {
                    SpawnInsect(item.gardenPrefab);
                }
            }
        }
    }

    private void SpawnInsect(Transform prefab)
    {
        Vector3 spawnPosition = targetTransform.position + Vector3.up * UnityEngine.Random.Range(1f, 3f) + UnityEngine.Random.insideUnitSphere * 2f;
        Insect newInsect = Instantiate(prefab, spawnPosition, Quaternion.identity).GetComponent<Insect>();

        newInsect.Spawn(targetTransform.position);
    }

    public void StartDecayTimer(float elapsedTime, float logoutTime)
    {
        float currentTime = GetCurrentDateTimeAsFloat();
        float timeSinceLastSave = currentTime - logoutTime;

        float remainingTime = plantSO.decayTime - elapsedTime - timeSinceLastSave;

        if (remainingTime <= 0)
        {
            RemoveFromPlotAndDestroy();
        }
        else
        {
            startTime = Time.time - elapsedTime; // Set the start time based on the elapsed time
            if (decayCoroutine != null)
            {
                StopCoroutine(decayCoroutine);
            }
            decayCoroutine = StartCoroutine(DecayTimer(remainingTime));
        }
    }

    public float GetElapsedTime()
    {
        return Time.time - startTime; // Calculate the elapsed time
    }

    private IEnumerator DecayTimer(float remainingTime)
    {
        while (remainingTime > 0)
        {
            yield return null; // Wait for the next frame
            remainingTime -= Time.deltaTime; // Decrease the remaining time by the delta time of the last frame
        }
        RemoveFromPlotAndDestroy();
    }

    private void RemoveFromPlotAndDestroy()
    {
        if (currentPlot != null)
        {
            currentPlot.RemovePlant(this);
        }
        Destroy(gameObject);
    }

    // Method to get the current date and time as a float
    public static float GetCurrentDateTimeAsFloat()
    {
        // Get the current date and time
        DateTime now = DateTime.UtcNow;

        // Define the Unix epoch (January 1, 1970)
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Calculate the difference between now and the Unix epoch
        TimeSpan timeSpan = now - epoch;

        // Return the total seconds as a float
        return (float)timeSpan.TotalSeconds;
    }
}
