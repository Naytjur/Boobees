using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
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
    private Transform[] buildings;

    //Internal Variables
    private Grid buildGrid;
    private bool isActive = false;
    private Transform currentBuilding;

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        buildGrid = new Grid(gridWidth, gridHeight, gridCellSize, gridOrigin.position);

        currentBuilding = buildings[0];
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
        buildGrid.GetXZ(Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface")), out int x, out int z);

        if(buildGrid.GetValue(x, z) == 0)
        {
            Instantiate(currentBuilding, buildGrid.GetWorldPosition(x, z) ,Quaternion.identity);
            buildGrid.SetValue(x, z, 1);
        }
    }

    //Getters and Setters
    public void SetCurrentBuilding(int index)
    {
        currentBuilding = buildings[index];
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
    }
}
