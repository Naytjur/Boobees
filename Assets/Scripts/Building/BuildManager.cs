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

    [Header("Build Hovering")]
    //Hovering visuals
    private bool hasPositionOnGrid = false;
    private Vector3 lastTarget;
    private Transform visual;
    private List<MeshRenderer> visualRenderers;
    [SerializeField]
    private Color ableColor;
    [SerializeField]
    private Color unableColor;
    private BuildingSO.Dir direction = BuildingSO.Dir.Down;

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

        if(Input.GetKeyDown(KeyCode.R))
        {
            direction = BuildingSO.GetNextDir(direction);
        }
    }

    private void LateUpdate()
    {
        if (visual != null)
        {
            UpdateVisual();

            if (Input.GetMouseButtonUp(0))
            {
                visual.gameObject.SetActive(true);
            }
        }
    }

    private void PlaceBuilding()
    {
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos) && currentBuilding != null)
        {
            buildGrid.GetXZ(pos, out int x, out int z);

            if (!CheckValidBuildPosition(currentBuilding, x, z))
            {
                return;
            }

            Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);
            Building buildObject = Building.Create(buildGrid.GetWorldPosition(x + rotationOffset.x, z + rotationOffset.y), new Vector2Int(x, z), currentBuilding, gridCellSize, Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0));

            if(visual != null)
            {
                visual.gameObject.SetActive(false);
            }

            foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z), direction))
            {
                buildGrid.SetValue(cell.x, cell.y, buildObject);
            }
        }
    }

    private bool CheckValidBuildPosition(BuildingSO building, int x, int z)
    {
        foreach (Vector2Int cell in building.GetGridPositions(new Vector2Int(x, z), direction))
        {
            if (!buildGrid.IsPositionOnGrid(cell.x, cell.y) || buildGrid.GetGridObject(cell.x, cell.y).building != null)
            {
                return false;
            }
        }
        return true;
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
            if(visualRenderers != null)
            {
                visualRenderers.Clear();
            }

            visual = Instantiate(buildingObject.visual, new Vector3(-1000, -1000, -1000), Quaternion.identity);
            visualRenderers = new List<MeshRenderer>(visual.GetComponentsInChildren<MeshRenderer>());
            visual.localScale = new Vector3(gridCellSize, gridCellSize, gridCellSize);
        }
    }

    private void UpdateVisual()
    {
        Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);

        visual.transform.position = Vector3.Lerp(visual.transform.position, lastTarget + new Vector3(rotationOffset.x, 0, rotationOffset.y), Time.deltaTime * 20f);
        visual.transform.rotation = Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0);

        if (GetMouseGridPosition(out Vector3 hit))
        {
            buildGrid.GetXZ(hit, out int x, out int z);

            foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z), direction))
            {
                if (!buildGrid.IsPositionOnGrid(cell.x, cell.y))
                {
                    return;
                }
            }

            lastTarget = hit;

            if(!CheckValidBuildPosition(currentBuilding, x, z))
            {
                foreach(MeshRenderer renderer in visualRenderers)
                {
                    renderer.material.SetColor("_Color", unableColor);
                }
            }
            else
            {
                foreach (MeshRenderer renderer in visualRenderers)
                {
                    renderer.material.SetColor("_Color", ableColor);
                }
            }

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
