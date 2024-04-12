using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlotType 
{ 
    Plot1, 
    Plot2, 
    Plot3
}

public class Plot : MonoBehaviour
{
    public int plantAmount;
    public int maxPlants;

    public List<Plant> plants = new List<Plant>();

    public PlotType type;
    public void AddPlant(Plant plant)
    {
        plants.Add(plant);
        plantAmount++;
    }

    public void RemovePlant(Plant plant)
    {
        plants.Remove(plant);
        plantAmount--;
    }

    public void ClearPlot()
    {
        plants.Clear();
        plantAmount = 0;
    }

    public bool IsFull()
    {
        return plantAmount >= maxPlants;
    }
}
