using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patch : Building
{
    private Plot plot;

    [SerializeField]
    private GameObject plantingSurface;

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
        PlantingManager.currentPlot = plot;
        GameManager.instance.UpdateGameState(GameState.Planting);
        FocusCamera();
        plantingSurface.SetActive(true);
    }

    private void FocusCamera()
    {
        Camera.main.transform.position = gameObject.transform.position + new Vector3(1.5f, 1f, -.5f);
        Camera.main.transform.eulerAngles = new Vector3(30, -45, 0);
    }

    private void OnGameStateChange(GameState state)
    {
        plantingSurface.SetActive(false);
    }
}
