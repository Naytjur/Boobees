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
            Building building = BuildManager.instance.buildGrid.GetGridObject(tile.x, tile.y).building;

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
}
