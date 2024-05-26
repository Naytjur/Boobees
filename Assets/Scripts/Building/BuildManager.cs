using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour, IDataPersistence
{
    public static BuildManager instance;

    private enum BuildState
    {
        Unselected,
        Placing,
        Moving
    }

    private BuildState state;

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
    private List<BuildingSO> allBuildings =  new List<BuildingSO>();

    //Internal Variables
    public Grid buildGrid;
    private bool isActive;
    private BuildingSO currentBuilding;
    private GameObject currentMover;

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
    public static event Action onBuildingPlaced;
    public static event Action onBuildingMoved;
    public static event Action onBuildingRemoved;

    private void Awake()
    {
        instance = this;
        confirmButton.onClick.AddListener(PlaceBuilding);
        rotateButton.onClick.AddListener(RotateBuilding);
        buildGrid = new Grid(gridWidth, gridHeight, gridCellSize, gridOrigin.position);
    }

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        ScoreManager.onLevelUp += OnLeveledUp;
        onBuildingChanged += ChangeVisual;
        UpdateActiveState(GameManager.instance.state);

        foreach(BuildingSO building in allBuildings)
        {
            building.RemoveCount(building.count);

        }
    }

    private void Update()
    {
        if (visual != null && lastTarget != null && isActive)
        {
            buildGrid.GetXZ(lastTarget, out int x, out int z);
            confirmButton.interactable = CheckValidBuildPosition(currentBuilding, x, z);
            UpdateVisual();
        }
    }

    public void LoadData(GameData data)
    {
        for(int i = 0; i < buildGrid.GetGridWidth(); i++)
        {
            for(int j = 0; j < buildGrid.GetGridHeight(); j++)
            {
                if(buildGrid.GetGridObject(i, j).building == null && data.buildGrid[GetBuildIndex(i, j)].buildingID != "empty")
                {
                    PlaceBuilding(i, j, data.buildGrid[GetBuildIndex(i, j)].buildingID, data.buildGrid[GetBuildIndex(i, j)].buildingRotation);
                }
            }
        }
    }

    private int GetBuildIndex(int x, int y)
    {
        int index = y * buildGrid.GetGridWidth() + x;
        return index;
    }

    public void SaveData(ref GameData data)
    {
        for(int i = 0; i < buildGrid.GetGridWidth(); i++)
        {
            for(int j = 0; j < buildGrid.GetGridHeight(); j++)
            {
                if(buildGrid.GetGridObject(i, j).building != null)
                {
                    data.buildGrid[GetBuildIndex(i, j)] = new BuildData(buildGrid.GetGridObject(i, j).building.buildingSO.id, buildGrid.GetGridObject(i, j).rotation);
                    Debug.Log(buildGrid.GetGridObject(i, j).building.buildingSO.id);
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && state != BuildState.Unselected && isActive)
        {
            UpdateVisualPosition();
        }
    }

    //Building Placement
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
            buildGrid.SetValue(cell.x, cell.y, buildObject, (int) direction);
        }

        ToggleHoveringUI(false);

        currentBuilding.AddCount(1);

        onBuildingPlaced?.Invoke();
    }

    private void PlaceBuilding(int x, int z, string id, int rotation)
    {
        BuildingSO cur = GetBuildingByID(id);
        Vector2Int rotationOffset = cur.GetRotationOffset((BuildingSO.Dir) rotation);
        Building buildObject = Building.Create(buildGrid.GetWorldPosition(x + rotationOffset.x, z + rotationOffset.y), new Vector2Int(x, z), cur, gridCellSize, Quaternion.Euler(0, cur.GetRotationDegrees((BuildingSO.Dir) rotation), 0));

        if(visual != null)
        {
            visual.gameObject.SetActive(false);
        }

        foreach (Vector2Int cell in cur.GetGridPositions(new Vector2Int(x, z), direction))
        {
            buildGrid.SetValue(cell.x, cell.y, buildObject, rotation);
        }

        cur.AddCount(1);

        onBuildingPlaced?.Invoke();
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
    public void SetBuildingByIndex(int index)
    {
        if (index < allBuildings.Count)
        {
            SetCurrentBuilding(allBuildings[index]);
            state = BuildState.Placing;
        }
    }

    public void SetCurrentBuilding(BuildingSO building)
    {
        currentBuilding = building;
        onBuildingChanged?.Invoke();

    }

    public BuildingSO GetCurrentBuilding()
    {
        return currentBuilding;
    }

    public List<BuildingSO> GetBuildingList()
    {
        return allBuildings;
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
        allBuildings.Add(building);
    }

    private BuildingSO GetBuildingByID(string id)
    {
        BuildingSO build = allBuildings[0];

        foreach(BuildingSO building in allBuildings)
        {
            if(building.id == id)
            {
                build = building;
                return build;
            }
        } 
        return build;
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

            visual = Instantiate(buildingObject.gardenVisual, lastTarget, Quaternion.identity);
            visualRenderers = new List<MeshRenderer>(visual.GetComponentsInChildren<MeshRenderer>());
            visual.localScale = new Vector3(gridCellSize, gridCellSize, gridCellSize);
        }

        ToggleHoveringUI(true);
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

    private void HandleClick()
    {
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos))
        {       
            buildGrid.GetXZ(pos, out int x, out int z);

            if (buildGrid.IsPositionOnGrid(x, z) && buildGrid.GetGridObject(x, z).building != null)
            {
                MoveBuilding(buildGrid.GetGridObject(x, z).building.buildingSO);
            }
        }
    }

    private void MoveBuilding(BuildingSO building)
    {
        state = BuildState.Moving;
        SetCurrentBuilding(building);
    }

    private void ToggleHoveringUI(bool val)
    {
        confirmButton.gameObject.SetActive(val);
        rotateButton.gameObject.SetActive(val);
        buildingSelectMenu.gameObject.SetActive(!val);
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
        this.state = BuildState.Unselected;
        confirmButton.gameObject.SetActive(false);
        rotateButton.gameObject.SetActive(false);
        buildingSelectMenu.gameObject.SetActive(true);
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }
    }

    private void OnLeveledUp(int level)
    {
        foreach(BuildingSO building in allBuildings)
        {
            building.TryUnlock(level);
            building.TryAdjustMaxCount(level);
        }
    }
}
