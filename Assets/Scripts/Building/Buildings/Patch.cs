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
        PlantingManager.instance.currentPlot = plot;
        GameManager.instance.UpdateGameState(GameState.Planting);
        FocusCamera();
        plantingSurface.SetActive(true);
    }

    private void FocusCamera()
    {
        Camera.main.transform.position = cameraPosition.position;
        Camera.main.transform.rotation = cameraPosition.rotation;
    }

    private void OnGameStateChange(GameState state)
    {
        plantingSurface.SetActive(false);
    }
}
