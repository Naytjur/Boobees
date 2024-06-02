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
    [SerializeField]
    private Transform center;

    private List<BuildingSO> modifierBuildings = new List<BuildingSO>(); 

    private void Start()
    {
        GameManager.instance.onStateChange += OnGameStateChange;
        BuildManager.onBuildingPlaced += UpdateModifierBuildingList;
        plot = GetComponent<Plot>();
    }
    public override void OnClick()
    {
        base.OnClick();
        PlantOnPatch();
    }

    private void PlantOnPatch()
    {
        if(GameManager.instance.state == GameState.PlotSelect)
        {
            PlantingManager.instance.currentPlot = plot;
            GameManager.instance.UpdateGameState(GameState.Planting);
            FocusCamera();
            plantingSurface.SetActive(true);
        }
    }

    private void FocusCamera()
    {
        Camera.main.transform.position = center.position + new Vector3(-1.2f, 1.2f, -1.2f);
        //Camera.main.transform.rotation = cameraPosition.rotation;
    }

    private void OnGameStateChange(GameState state)
    {
        if(plantingSurface != null)
        {
            plantingSurface.SetActive(false);
        }
    }

    private void UpdateModifierBuildingList()
    {
        modifierBuildings.Clear();

        foreach(Building building in GetSurroundingBuildings())
        {
            if(!modifierBuildings.Contains(building.buildingSO))
            {
                modifierBuildings.Add(building.buildingSO);
            }
        }
    }

    private List<Building> GetSurroundingBuildings()
    {
        List<Building> buildings = new List<Building>();

        foreach(Vector2Int tile in GetSurroundingTiles())
        {
            Building building = null;

            if(BuildManager.instance.buildGrid.GetGridObject(tile.x, tile.y, out GridObject gridObject))
            {
                building = gridObject.building;
            }

            if(building != null)
            {
                if(!buildings.Contains(building))
                {
                    buildings.Add(building);
                }
            }

        }
        return buildings;
    }

    public void LoadPlants(List<PlantData> plants)
    {
        if(plot == null)
        {
            plot = GetComponent<Plot>();
        }

        foreach(PlantData data in plants)
        {
            Transform plantTransform = Instantiate(PlantingManager.instance.GetPlantByID(data.plantID).gardenPrefab, plot.transform);
            plantTransform.position = new Vector3(data.x, data.y, data.z);

            Plant plant = plantTransform.GetComponent<Plant>();
            PlantingManager.instance.plantList.Add(plant);
            plot.AddPlant(plant);
            plant.AssignPlot(plot.type);
        }
    }

    public void SavePlants()
    {
        foreach(Plant plant in plot.plants)
        {
            buildData.placedPlants.Add(new PlantData(plant.plantSO.id, plant.transform.position.x, plant.transform.position.y, plant.transform.position.z));
        }
    }
}
