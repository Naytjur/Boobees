using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Plant", menuName = "Create Plant")]
[System.Serializable]
public class PlantSO : CatalogueItemInfo
{
    public PlotType[] correctPlots;
    public float baseSpawnRate;
    public int seedAmount;
    public float decayTime;
    public DateTime lastSeedsGainedTime = DateTime.MinValue;

    public void SetDecayTime(float newDecayTime)
    {
        this.decayTime = newDecayTime;
    }

    public bool CanGainSeeds()
    {
        return (DateTime.Now - lastSeedsGainedTime).TotalMinutes >= 15;
    }

    public void GainSeeds(int seedsGained)
    {
        if (CanGainSeeds())
        {
            seedAmount += seedsGained;
            lastSeedsGainedTime = DateTime.Now;
        }
        else
        {
            Debug.Log("Too soon to gain seeds for this plant.");
        }   
    }
}
