using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public float gridCellSize;

    [Header("Buildings")]
    [SerializeField]
    private List<BuildingSO> buildings =  new List<BuildingSO>();

    //Internal Variables
    public Grid buildGrid;
    private bool isActive;
    private BuildingSO currentBuilding;

    //Hovering visuals
    private bool hasPositionOnGrid = false;
    private Vector3 lastTarget;
    private Transform visual;

    public static event Action onBuildingChanged;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        GameManager.instance.onStateChange += Toggle;
        onBuildingChanged += ChangeVisual;
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

    private void LateUpdate()
    {
        if (visual != null)
        {
            UpdateVisual();
        }
    }

    private void PlaceBuilding()
    {
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos) && currentBuilding != null)
        {
            buildGrid.GetXZ(pos, out int x, out int z);

            foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z)))
            {
                if(!buildGrid.IsPositionOnGrid(cell.x, cell.y) || buildGrid.GetGridObject(cell.x, cell.y).building != null)
                {
                    return;
                }
            }
            
            Building buildObject = Building.Create(buildGrid.GetWorldPosition(x, z), new Vector2Int(x, z), currentBuilding, gridCellSize);

            foreach(Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z)))
            {
                buildGrid.SetValue(cell.x, cell.y, buildObject);
            }
        }
    }

    //Getters and Setters
    public void SetCurrentBuilding(int index)
    {
        if(index < buildings.Count)
        {
            currentBuilding = buildings[index];
            onBuildingChanged?.Invoke();
        }
    }

    public BuildingSO GetCurrentBuilding()
    {
        return currentBuilding;
    }

    public List<BuildingSO> GetBuildingList()
    {
        return buildings;
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

    public void AddBuilding(BuildingSO building)
    {
        buildings.Add(building);
    }

    //Hovering visual
    private void ChangeVisual()
    {
        hasPositionOnGrid = false;

        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        BuildingSO buildingObject = BuildManager.instance.GetCurrentBuilding();

        if (buildingObject != null && GameManager.instance.state == GameState.Building)
        {
            visual = Instantiate(buildingObject.visual, new Vector3(-1000, -1000, -1000), Quaternion.identity);
            visual.localScale = new Vector3(gridCellSize, gridCellSize, gridCellSize);
        }
    }

    private void UpdateVisual()
    {
        visual.transform.position = Vector3.Lerp(visual.transform.position, lastTarget, Time.deltaTime * 20f);

        if (GetMouseGridPosition(out Vector3 hit))
        {
            buildGrid.GetXZ(hit, out int x, out int z);

            foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z)))
            {
                if (!buildGrid.IsPositionOnGrid(cell.x, cell.y) || buildGrid.GetGridObject(cell.x, cell.y).building != null)
                {
                    return;
                }
            }

            lastTarget = hit;

            if (!hasPositionOnGrid)
            {
                visual.transform.position = hit;
                hasPositionOnGrid = true;
            }
        }
    }

    private void Toggle(GameState state)
    {
        ChangeVisual();
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
    }
}
