using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;
using UnityEngine.UI;

public class PlantingManager : MonoBehaviour, IDataPersistence
{
    public static PlantingManager instance;
    private enum PlantState
    {
        Unselected,
        Hovering,
        Planting
    }

    public List<PlantSO> allPlants;

    private PlantSO currentPlant;

    //hovering
    private Transform hoverVisual;
    private Vector3 hoverPosition;

    public Plot currentPlot;
    public List<Plant> plantList = new List<Plant>();

    public List<PlantData> plants = new List<PlantData>();

    //UI
    public Button confirmButton;
    public Button clearButton;
    public TMP_Text plantAmount;
    public Transform plantSelectionPanel;

    //State
    public bool isPlanting = false;
    private PlantState plantState = PlantState.Unselected;

    //events
    public event Action<PlantSO> onPlantUnlocked;
    public event Action onPlantPlanted;

    public LocalizeStringEvent plotFullEvent;
    public LocalizedString plotFullMessage;


    private void Awake()
    {
        instance = this;
        DataPersistenceManager.postLoad += UpdatePlants;
    }
    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        ScoreManager.onLevelUp += OnLeveledUp;
        confirmButton.onClick.AddListener(Plant);
        clearButton.onClick.AddListener(ClearCurrentPlot);
    }

    void Update()
    {
        if (isPlanting && !currentPlot.IsFull() && currentPlant != null)
        {
            CheckHover();
        }

        confirmButton.gameObject.SetActive(plantState != PlantState.Unselected);
    }

    //Save & Load stuff
    public void LoadData(GameData data)
    {

        foreach (PlantData plant in data.plantList)
        {
            LoadPlant(plant.worldX, plant.worldY, plant.worldZ, plant.plantID, plant.plantRotation);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.plantList.Clear();
        foreach(PlantData plant in plants)
        {
            data.plantList.Add(plant);
        }
    }

    private void CheckHover()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (plantState == PlantState.Hovering)
            {
                PlacePlant();
                plantSelectionPanel.gameObject.SetActive(true);
            }
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPosition = Input.mousePosition;

            if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("PlantSurface"), out Vector3 pos))
            {
                hoverPosition = pos;
                Hover(hoverPosition);
                plantState = PlantState.Hovering;
                confirmButton.interactable = false;
                plantSelectionPanel.gameObject.SetActive(false);
            }
        }
    }

    public void StartPlanting()
    {
        plantState = PlantState.Unselected;
        confirmButton.interactable = false;
        isPlanting = true;
        UpdateAmountUI();
    }

    private void PlacePlant()
    {
        plantState = PlantState.Planting;
        confirmButton.interactable = true;
    }

    public void Plant()
    {
        clearButton.interactable = true;

        if (plantState == PlantState.Planting && !currentPlot.IsFull())
        {
            Transform plantTransform = Instantiate(currentPlant.gardenPrefab, currentPlot.transform);
            plantTransform.position = hoverPosition;

            Plant plant = plantTransform.GetComponent<Plant>();
            plant.plantSO.seedAmount -= 1;
            plantList.Add(plant);
            currentPlot.AddPlant(plant);
            Destroy(hoverVisual.gameObject);
            plant.AssignPlot(currentPlot.type);
            
            if(plant.plantSO.seedAmount <= 0)
            {
                StopPlanting();
            }

            UpdateAmountUI();

            onPlantPlanted?.Invoke();
        }

        StartPlanting();
    }
    private void Hover(Vector3 location)
    {
        if (hoverVisual == null)
        {
            hoverVisual = Instantiate(currentPlant.gardenVisual, currentPlot.transform);
        }

        hoverVisual.transform.position = location;
    }

    public void ClearCurrentPlot()
    {
        foreach (Plant plant in currentPlot.plants)
        {
            plantList.Remove(plant);
            Destroy(plant.gameObject);
        }

        currentPlot.ClearPlot();

        UpdateAmountUI();

        StopPlanting();
        clearButton.interactable = false;
        plantState = PlantState.Unselected;
        StartPlanting();
    }

    public void StopPlanting()
    {
        isPlanting = false;

        if (plantState != PlantState.Unselected && hoverVisual != null)
        {
            Destroy(hoverVisual.gameObject);
            currentPlant = null;
        }
    }

    private void UpdateAmountUI()
    {
        plantAmount.text = currentPlot.plantAmount.ToString() + "/" + currentPlot.maxPlants.ToString();

        if (currentPlot.plantAmount >= currentPlot.maxPlants)
        {
            plotFullEvent.StringReference = plotFullMessage;
            //plantAmount.text = "Full!";
        }
    }

    public void SwitchPlant(int index)
    {
        if (allPlants[index].unlocked)
        {
            currentPlant = allPlants[index];

            if(hoverVisual != null)
            {
                Destroy(hoverVisual.gameObject);
                hoverVisual = Instantiate(allPlants[index].gardenVisual);
                hoverVisual.position = hoverPosition;
            }
        }
    }

    //Unlocking plants
    public bool TryUnlockPlant(string id, out string name)
    {
        foreach(PlantSO plant in allPlants)
        {
            if (plant.id == id)
            {
                if(!plant.unlocked)
                {
                    UnlockPlant(plant);     
                }
                GetSeeds(plant);
                name = plant.name;
                return true;
            }
        }
        name = "Invalid ID";
        return false;
    }

    public void UnlockPlant(PlantSO plant)
    {
        plant.unlocked = true;
        onPlantUnlocked?.Invoke(plant);
    }

    private void GetSeeds(PlantSO plant)
    {
        plant.seedAmount += 3;
    }

    private void UpdateActiveState(GameState state)
    {
        switch (state)
        {
            case GameState.Viewing:
                StopPlanting();
                break;
            case GameState.Planting:
                StartPlanting();
                break;
            case GameState.Building:
                StopPlanting();
                break;
        }
    }

    //Setters and Getters
    public List<PlantSO> GetPlantList()
    {
        return allPlants;
    }

    public PlantSO GetPlantByIndex(int index)
    {
        return allPlants[index];
    }

    public void LoadPlant(float x, float y, float z, string id, int rotation)
    {

    }

    public void UpdatePlants()
    {

    }

    public void OnLeveledUp(int level)
    {
        UpdatePlants();
    }
}
