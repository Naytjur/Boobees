using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patch : Building
{
    private Plot plot;

    [SerializeField]
    private GameObject plantingSurface;
    [SerializeField]
    private Transform cameraPosition;
    [SerializeField]
    private Transform center;

    private void Start()
    {
        GameManager.instance.onStateChange += OnGameStateChange;
        plot = GetComponent<Plot>();
    }
    public override void OnClick()
    {
        base.OnClick();
        PlantOnPatch();
    }

    private void PlantOnPatch()
    {
        if(GameManager.instance.state == GameState.PlotSelect)
        {
            PlantingManager.instance.currentPlot = plot;
            GameManager.instance.UpdateGameState(GameState.Planting);
            FocusCamera();
            plantingSurface.SetActive(true);
        }
    }

    private void FocusCamera()
    {
        Camera.main.transform.position = center.position + new Vector3(-1.2f, 1.2f, -1.2f);
        //Camera.main.transform.rotation = cameraPosition.rotation;
    }

    private void OnGameStateChange(GameState state)
    {
        plantingSurface.SetActive(false);
    }
}
