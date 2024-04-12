using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    public List<GameObject> insects = new List<GameObject>();
    public PlantSO plantSO;
    public PlotType plot;
    public bool isOnCorrectPlot;

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
}
