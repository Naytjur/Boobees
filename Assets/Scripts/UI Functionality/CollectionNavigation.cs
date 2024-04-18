using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionNavigation : MonoBehaviour
{
    [SerializeField]
    private GameObject catalogue;
    [SerializeField]
    private GameObject mainPage;
    [SerializeField]
    private GameObject plantsPage;
    [SerializeField]
    private GameObject plantPage;
    [SerializeField]
    private GameObject insectsPage;

    [SerializeField]
    private TMP_Text plantName;
    [SerializeField]
    private TMP_Text plantDescription;

    private GameObject currentPage;

    [SerializeField]
    private Transform plantButtonContainer;

    private void Start()
    {
        currentPage = mainPage;
        GameManager.instance.onStateChange += OnStateChanged;
    }

    public void OpenCatalogue()
    {
        catalogue.SetActive(true);
        ToMainPage();
    }

    public void CloseCatalogue()
    {
        catalogue.SetActive(false);
    }

    public void ToMainPage()
    {
        currentPage.SetActive(false);
        mainPage.SetActive(true);
        plantsPage.SetActive(false); //idk why its enabling this the first time catolog is opened, but adding this fixed it * Luiz Band-Aid TM
        currentPage = mainPage;
    }

    public void ToPlantsPage()
    {
        currentPage.SetActive(false);
        plantsPage.SetActive(true);
        currentPage = plantsPage;
        UpdatePlantButtons();
    }

    public void ToInsectsPage()
    {
        currentPage.SetActive(false);
        insectsPage.SetActive(true);
        currentPage = insectsPage;
    }

    public void ToPlantPage(PlantSO plant)
    {
        currentPage.SetActive(false);
        plantPage.SetActive(true);
        currentPage = plantPage;

        plantName.text = plant.plantName;
        plantDescription.text = plant.description;
    }

    private void UpdatePlantButtons()
    {
        foreach(Transform child in plantButtonContainer)
        {
            Button button = child.GetComponent<Button>();
            button.interactable = button.GetComponent<SelectPlantButton>().plant.unlocked;
        }
    }

    private void OnStateChanged(GameState state)
    {
        if(state == GameState.Catalogue)
        {
            OpenCatalogue();
        }
        else
        {
            CloseCatalogue();
        }
    }
}
