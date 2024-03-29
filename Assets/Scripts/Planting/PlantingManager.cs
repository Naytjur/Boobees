using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlantingManager : MonoBehaviour
{
    private enum PlantState
    {
        Unselected,
        Hovering,
        Planting
    }

    public Transform plantPrefab;
    private Transform instance;

    public static Plot currentPlot;

    public List<Plant> plantList = new List<Plant>();

    //UI
    public Button plantButton;
    public Button clearButton;
    public TMP_Text plantAmount;

    public bool isPlanting = false;
    private bool hasInstance;

    private PlantState plantState = PlantState.Unselected;

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        plantButton.onClick.AddListener(Plant);
        clearButton.onClick.AddListener(ClearCurrentPlot);
    }

    void Update()
    {
        if (isPlanting && !currentPlot.IsFull())
        {
            CheckHover();
        }
        plantButton.gameObject.SetActive(plantState != PlantState.Unselected);
    }

    private void CheckHover()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (plantState == PlantState.Hovering)
            {
                PlacePlant();
            }
            return;
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 screenPosition = Input.mousePosition;

            if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("PlantSurface"), out Vector3 pos))
            {
                Hover(pos);
                plantState = PlantState.Hovering;
                plantButton.interactable = false;
            }
        }
    }

    public void StartPlanting()
    {
        plantState = PlantState.Unselected;
        plantButton.interactable = false;
        isPlanting = true;
        UpdateAmountUI();
    }

    private void PlacePlant()
    {
        plantState = PlantState.Planting;
        plantButton.interactable = true;
    }

    public void Plant()
    {
        clearButton.interactable = true;

        if (plantState == PlantState.Planting && !currentPlot.IsFull())
        {
            Plant plant = instance.GetComponent<Plant>();
            plantList.Add(plant);
            currentPlot.AddPlant(plant);
            hasInstance = false;
            isPlanting = false;
            UpdateAmountUI();
        }

        StartPlanting();
    }

    private void Hover(Vector3 location)
    {
        if (hasInstance == false)
        {
            instance = Instantiate(plantPrefab);
            hasInstance = true;
        }

        instance.transform.position = location;
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

        if (plantState != PlantState.Unselected)
        {
            Destroy(instance);
        }

        hasInstance = false;
    }

    private void UpdateAmountUI()
    {
        plantAmount.text = "(" + currentPlot.plantAmount.ToString() + "/" + currentPlot.maxPlants.ToString() + ")" + " plants";

        if (currentPlot.plantAmount >= currentPlot.maxPlants)
        {

            plantAmount.text = "Full!";
        }
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        switch (state
)
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
}