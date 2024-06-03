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
            spawnRate *= 0.6f;
        }
        // Apply modifiers from surrounding buildings
        spawnRate = ApplySpawnRateModifiers(spawnRate);

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
                float rarityPercentage = insect.GetRarityPercentage();
                rarityPercentage = ApplyRarityModifiers(insect, rarityPercentage);

                if (UnityEngine.Random.Range(1, 101) <= rarityPercentage)
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

        private float ApplySpawnRateModifiers(float baseSpawnRate)
    {
        foreach (BuildingSO building in GetModifierBuildings())
        {
            foreach (InsectModifier modifier in building.insectModifiers)
            {
                foreach (ItemInfo attraction in plantSO.attractions)
                {
                    if (attraction is InsectSO insect && (modifier.insectType == insect.type || modifier.insectType == InsectSO.Type.Bee || modifier.insectType == InsectSO.Type.Butterfly))
                    {
                        if (modifier.modifySpawnRate)
                        {
                            baseSpawnRate *= modifier.modifierAmount;  // Adjust the spawn rate
                        }
                    }
                }
            }
        }
        return baseSpawnRate;
    }

    private float ApplyRarityModifiers(InsectSO insect, float baseRarity)
    {
        foreach (BuildingSO building in GetModifierBuildings())
        {
            foreach (InsectModifier modifier in building.insectModifiers)
            {
                foreach (ItemInfo attraction in plantSO.attractions)
                {
                    if (attraction is InsectSO insectAttraction && (modifier.insectType == insectAttraction.type || modifier.insectType == InsectSO.Type.Bee || modifier.insectType == InsectSO.Type.Butterfly))
                    {
                        if (!modifier.modifySpawnRate)
                        {
                            baseRarity *= modifier.modifierAmount;  // Adjust the rarity
                        }
                    }
                }
            }
        }
        return baseRarity;
    }


    private List<BuildingSO> GetModifierBuildings()
    {
        Patch patch = GetComponentInParent<Patch>();
        return patch != null ? patch.GetModifierBuildings() : new List<BuildingSO>();
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
        PlantingManager.instance.UpdateAmountUI();
        plantSO.seedAmount++;
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
