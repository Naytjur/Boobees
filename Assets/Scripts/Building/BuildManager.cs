using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

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

    //UI
    [Header("UI Assignments")]
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button rotateButton;
    [SerializeField]
    private GameObject buildingSelectMenu;


    public static event Action onBuildingChanged;

    private void Awake()
    {
        instance = this;
        confirmButton.onClick.AddListener(PlaceBuilding);
        rotateButton.onClick.AddListener(RotateBuilding);
    }

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        onBuildingChanged += ChangeVisual;
        UpdateActiveState(GameManager.instance.state);
        buildGrid = new Grid(gridWidth, gridHeight, gridCellSize, gridOrigin.position);
    }

    private void Update()
    {
        if (visual != null && lastTarget != null)
        {
            buildGrid.GetXZ(lastTarget, out int x, out int z);
            confirmButton.interactable = CheckValidBuildPosition(currentBuilding, x, z);
            UpdateVisual();
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && visual != null)
        {
            UpdateVisualPosition();
        }
    }

    private void PlaceBuilding()
    {
        buildGrid.GetXZ(lastTarget, out int x, out int z);

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

        confirmButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        buildingSelectMenu.gameObject.SetActive(true);
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

            visual = Instantiate(buildingObject.visual, lastTarget, Quaternion.identity);
            visualRenderers = new List<MeshRenderer>(visual.GetComponentsInChildren<MeshRenderer>());
            visual.localScale = new Vector3(gridCellSize, gridCellSize, gridCellSize);
        }

        confirmButton.gameObject.SetActive(true);
        rotateButton.gameObject.SetActive(true);
        buildingSelectMenu.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);

        visual.transform.position = Vector3.Lerp(visual.transform.position, lastTarget + new Vector3(rotationOffset.x, 0, rotationOffset.y), Time.deltaTime * 20f);
        visual.transform.rotation = Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0);
    }

    private void UpdateVisualPosition()
    {
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

            if (!CheckValidBuildPosition(currentBuilding, x, z))
            {
                foreach (MeshRenderer renderer in visualRenderers)
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


    private void RotateBuilding()
    {
        direction = BuildingSO.GetNextDir(direction);
        UpdateVisual();
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
        ChangeVisual();
        confirmButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        buildingSelectMenu.gameObject.SetActive(true);
    }
}
