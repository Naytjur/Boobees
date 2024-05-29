using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Viewing,
    PlotSelect,
    Planting,
    Building,
    Catalogue,
    Scanning,
    Map
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state { private set; get; }
    private GameState previousState;

    public event Action<GameState> onStateChange;

    public CameraPosition cameraPositon;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state =  GameState.Viewing;
        previousState = GameState.Viewing;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    public void UpdateGameState(GameState newState)
    {
        if (state == newState)
        {
            return;
        }

        previousState = state;
        state = newState;

        switch (newState)
        {
            case GameState.Viewing:
                HandleViewing();
                break;
            case GameState.Planting:
                HandlePlanting();
                break;
            case GameState.Building:
                HandleBuilding();
                break;
            case GameState.Catalogue:
                HandleCatalogue();
                break;
            case GameState.Scanning:
                HandleScanning();
                break;
            case GameState.PlotSelect:
                HandlePlotSelect();
                break;
            case GameState.Map:
                HandleMap();
                break;
        }

        onStateChange?.Invoke(newState);
    }

    private void HandleMap()
    {
        
    }

    private void HandlePlotSelect()
    {
        
    }

    public void ReturnToPreviousState()
    {
        if(previousState != state)
        {
            UpdateGameState(previousState);
        }
    }

    private void HandleCatalogue()
    {

    }

    private void HandleViewing()
    {
        cameraPositon.SetCameraDefaultPosition();
    }

    private void HandlePlanting()
    {
        Debug.Log("Now Planting");
    }

    private void HandleBuilding()
    {
        cameraPositon.SetCameraDefaultPosition();
    }

    private void HandleScanning()
    {

    }

    private void HandleClick()
    {
        if (state != GameState.Scanning)
        {
            if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos))
            {
                Grid grid = BuildManager.instance.buildGrid;
                grid.GetXZ(pos, out int x, out int z);
                if (grid.IsPositionOnGrid(x, z) && grid.GetGridObject(x, z).building != null)
                {
                    grid.GetGridObject(x, z).building.OnClick();
                }
            }
        }
    }
}

