using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [Header("Grid Configuration")]
    [SerializeField]
    private Transform gridOrigin;
    [SerializeField]
    private int gridWidth;
    [SerializeField]
    private int gridHeight;
    [SerializeField]
    private float gridCellSize;

    [Header("Buildings")]
    [SerializeField]
    private BuildingSO[] buildings;

    //Internal Variables
    public Grid buildGrid;
    private bool isActive;
    private BuildingSO currentBuilding;

    public static event Action onBuildingChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        UpdateActiveState(GameManager.instance.state);
        buildGrid = new Grid(gridWidth, gridHeight, gridCellSize, gridOrigin.position);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isActive)
        {
            PlaceBuilding();
        }
    }

    private void PlaceBuilding()
    {
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos) && currentBuilding != null)
        {
            buildGrid.GetXZ(pos, out int x, out int z);

            if (buildGrid.IsPositionOnGrid(x, z) && buildGrid.GetGridObject(x, z).building == null)
            {
                Building buildObject = Building.Create(buildGrid.GetWorldPosition(x, z), new Vector2Int(x, z), currentBuilding);

                buildGrid.SetValue(x, z, buildObject);
            }
        }
    }

    //Getters and Setters
    public void SetCurrentBuilding(int index)
    {
        if(index < buildings.Length)
        {
            currentBuilding = buildings[index];
            onBuildingChanged?.Invoke();
        }
    }

    public BuildingSO GetCurrentBuilding()
    {
        return currentBuilding;
    }

    public bool GetMouseGridPosition(out Vector3 hit)
    {
        hit = Vector3.zero;
        
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos))
        {
            buildGrid.GetXZ(pos, out int x, out int z);

            if (buildGrid.IsPositionOnGrid(x, z))
            {
                hit = buildGrid.GetWorldPosition(x, z);
                return true;
            }
        }
        return false;
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
    }
}
