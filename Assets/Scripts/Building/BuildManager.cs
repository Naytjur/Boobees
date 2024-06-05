using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuildManager : MonoBehaviour, IDataPersistence
{
    public static BuildManager instance;

    public enum BuildState
    {
        Unselected,
        Placing,
        Moving
    }

    public BuildState state;

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
    private List<Building> buildings = new List<Building>();

    //Internal Variables
    public Grid buildGrid;
    private bool isActive;
    private BuildingSO currentBuilding;
    private Building currentMover;

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

    [Header("UI")]
    [SerializeField]
    private Button confirmButton;


    public static event Action onBuildingChanged;
    public static event Action onBuildingPlaced;
    public static event Action onBuildingMoved;
    public static event Action onBuildingRemoved;
    public static event Action<BuildState> onStateChanged;

    private void Awake()
    {
        instance = this;
        buildGrid = new Grid(gridWidth, gridHeight, gridCellSize, gridOrigin.position);
    }

    private void OnEnable()
    {
        DataPersistenceManager.postLoad += UpdateBuildings;
    }

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        ScoreManager.onLevelUp += OnLeveledUp;
        onBuildingChanged += ChangeVisual;
        UpdateActiveState(GameManager.instance.state);
        UpdateBuildings();
    }

    private void Update()
    {
        if (currentBuilding != null && lastTarget != null && isActive)
        {
            buildGrid.GetXZ(lastTarget, out int x, out int z);
            confirmButton.interactable = CheckValidBuildPosition(currentBuilding, x, z);
            UpdateVisual();
        }
    }
    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && state != BuildState.Unselected && isActive)
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            UpdateVisualPosition();
        }
    }

    //Save Load
    public void LoadData(GameData data)
    {
        float logoutTime = data.logoutTime;

        foreach (BuildingSO building in allBuildings)
        {
            building.RemoveCount(building.count);
        }

        foreach (BuildData buildData in data.buildingList)
        {
            Building building = PlaceBuilding(buildData.gridX, buildData.gridZ, buildData.buildingID, buildData.buildingRotation);
            if (building is Patch patch)
            {
                patch.LoadPlants(buildData.placedPlants, logoutTime);
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
        data.buildingList.Clear();
        data.logoutTime = Plant.GetCurrentDateTimeAsFloat();

        foreach (Building building in buildings)
        {
            if (building is Patch patch)
            {
                patch.SavePlants();
            }

            data.buildingList.Add(building.buildData);
        }
    }

    //Building Placement
    public void StartPlacing(int index)
    {
        CancelCurrent();

        if (index < allBuildings.Count)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(PlaceBuilding);
            SetCurrentBuilding(allBuildings[index]);
            UpdateBuildState(BuildState.Placing);
        }
    }

    public void StartMoving(Building build)
    {
        CancelCurrent();
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(delegate { MoveBuilding(build); });
        SetCurrentMover(build);
        direction = (BuildingSO.Dir) build.buildData.buildingRotation;
        build.Hide();
        RemoveFromGrid(build);
        UpdateBuildState(BuildState.Moving);
    }

    private void PlaceBuilding()
    {
        buildGrid.GetXZ(lastTarget, out int x, out int z);

        if (!CheckValidBuildPosition(currentBuilding, x, z))
        {
            return;
        }

        BuildData data = new BuildData(currentBuilding.id, x, z, (int)direction);

        Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);
        Building buildObject = Building.Create(buildGrid.GetWorldPosition(x + rotationOffset.x, z + rotationOffset.y), new Vector2Int(x, z), currentBuilding, data, gridCellSize, Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0), direction);
        
        buildings.Add(buildObject);
        buildObject.buildData = data;

        if (visual != null)
        {
            visual.gameObject.SetActive(false);
        }

        foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z), direction))
        {
            buildGrid.SetValue(cell.x, cell.y, buildObject, (int) direction);
        }

        currentBuilding.AddCount(1);

        ScoreManager.instance.UpdateScores(0, -currentBuilding.cost);

        onBuildingPlaced?.Invoke();
        UpdateBuildState(BuildState.Unselected);
    }

    private Building PlaceBuilding(int x, int z, string id, int rotation)
    {
        BuildingSO cur = GetBuildingByID(id);
        Vector2Int rotationOffset = cur.GetRotationOffset((BuildingSO.Dir) rotation);
        BuildData data = new BuildData(id, x, z, rotation);

        Building buildObject = Building.Create(buildGrid.GetWorldPosition(x + rotationOffset.x, z + rotationOffset.y), new Vector2Int(x, z), cur, data, gridCellSize, Quaternion.Euler(0, cur.GetRotationDegrees((BuildingSO.Dir) rotation), 0), (BuildingSO.Dir) rotation);

        buildings.Add(buildObject);
        buildObject.buildData = data;

        if (visual != null)
        {
            visual.gameObject.SetActive(false);
        }

        foreach (Vector2Int cell in cur.GetGridPositions(new Vector2Int(x, z), (BuildingSO.Dir) rotation))
        {
            buildGrid.SetValue(cell.x, cell.y, buildObject, rotation);
        }

        cur.AddCount(1);

        onBuildingPlaced?.Invoke();

        return buildObject;
    }

    public void MoveBuilding(Building building)
    {
        buildGrid.GetXZ(lastTarget, out int x, out int z);

        if (!CheckValidBuildPosition(currentBuilding, x, z))
        {
            return;
        }

        Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);

        RemoveFromGrid(building);

        building.Move(buildGrid.GetWorldPosition(x + rotationOffset.x, z + rotationOffset.y), new Vector2Int(x, z), Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0), direction);


        if (visual != null)
        {
            visual.gameObject.SetActive(false);
        }

        foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(x, z), direction))
        {
            buildGrid.SetValue(cell.x, cell.y, building, (int) direction);
        }

        onBuildingMoved?.Invoke();

        UpdateBuildState(BuildState.Unselected);
    }

    public void RemoveBuilding()
    {
        CancelCurrent();
        RemoveFromGrid(currentMover);
        buildings.Remove(currentMover);
        currentBuilding.AddCount(-1);
        ScoreManager.instance.UpdateScores(0, currentBuilding.cost / 2);
        Destroy(currentMover.gameObject);

        onBuildingRemoved?.Invoke();
        UpdateBuildState(BuildState.Unselected);
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

        BuildingSO buildingObject = currentBuilding;

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
    }

    private void UpdateVisual()
    {
        Vector2Int rotationOffset = currentBuilding.GetRotationOffset(direction);

        visual.transform.position = Vector3.Lerp(visual.transform.position, lastTarget + new Vector3(rotationOffset.x, 0, rotationOffset.y), Time.deltaTime * 20f);
        visual.transform.rotation = Quaternion.Euler(0, currentBuilding.GetRotationDegrees(direction), 0);

        buildGrid.GetXZ(lastTarget, out int x, out int z);
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

            if (!hasPositionOnGrid)
            {
                visual.transform.position = hit;
                hasPositionOnGrid = true;
            }
        }
    }

    public void RotateBuilding()
    {
        direction = BuildingSO.GetNextDir(direction);
        UpdateVisual();
    }

    //Helper
    private bool CheckValidBuildPosition(BuildingSO building, int x, int z)
    {
        if (building == null)
        {
            return false;
        }

        foreach (Vector2Int cell in building.GetGridPositions(new Vector2Int(x, z), direction))
        {
            if (!buildGrid.IsPositionOnGrid(cell.x, cell.y) || !buildGrid.GetGridObject(cell.x, cell.y, out GridObject tile) || tile.building != null)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveFromGrid(Building building)
    {
        foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(building.buildData.gridX, building.buildData.gridZ), (BuildingSO.Dir) building.buildData.buildingRotation))
        {
            buildGrid.SetValue(cell.x, cell.y, null, 0);
        }
    }

    private void CancelCurrent()
    {
        if(state == BuildState.Moving)
        {
            currentMover.Show();

            foreach (Vector2Int cell in currentBuilding.GetGridPositions(new Vector2Int(currentMover.buildData.gridX, currentMover.buildData.gridZ), (BuildingSO.Dir)currentMover.buildData.buildingRotation))
            {
                buildGrid.SetValue(cell.x, cell.y, currentMover, currentMover.buildData.buildingRotation);
            }
        }
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }
    }
    private void UpdateBuildState(BuildState state)
    {
        this.state = state;

        if(state == BuildState.Unselected)
        {
            currentMover = null;
            currentBuilding = null;
        }

        onStateChanged?.Invoke(state);
    }

    private void UpdateBuildings()
    {
        foreach (BuildingSO building in allBuildings)
        {
            building.TryUnlock(ScoreManager.instance.playerLevel);
            building.TryAdjustMaxCount(ScoreManager.instance.playerLevel);
        }
    }

    //Getters and Setters
    public void SetCurrentBuilding(BuildingSO building)
    {
        currentBuilding = building;
        onBuildingChanged?.Invoke();
    }

    public void SetCurrentMover(Building building)
    {
        currentMover = building;
        SetCurrentBuilding(building.buildingSO);
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
            pos += new Vector3(0.001f, 0, 0.001f);
            buildGrid.GetXZ(pos, out int x, out int z);

            if (buildGrid.IsPositionOnGrid(x, z))
            {
                hit = buildGrid.GetWorldPosition(x, z);
                return true;
            }
        }
        return false;
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

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
        UpdateBuildState(BuildState.Unselected);

        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }

        if(state != GameState.Building)
        {
            CancelCurrent();
        }
    }

    private void OnLeveledUp(int level)
    {
        UpdateBuildings();
    }
}
