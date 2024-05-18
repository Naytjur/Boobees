using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject catalogueUI;
    [SerializeField]
    private GameObject buildUI;
    [SerializeField]
    private GameObject plantUI;
    [SerializeField]
    private GameObject overviewUI;
    [SerializeField]
    private GameObject scanningUI;

    private void Start()
    {
        GameManager.instance.onStateChange += SwitchUI;
    }

    private void SwitchUI(GameState state)
    {
        HideAllUI();
        switch (state)
        {
            case GameState.Viewing:
                overviewUI.SetActive(true);
                break;
            case GameState.Planting:
                plantUI.SetActive(true);
                break;
            case GameState.Building:
                buildUI.SetActive(true);
                break;
            case GameState.Catalogue:
                catalogueUI.SetActive(true);
                break;
            case GameState.Scanning:
                scanningUI.SetActive(true);
                break;
        }
    }

    private void HideAllUI()
    {
        catalogueUI.SetActive(false);
        buildUI.SetActive(false);
        plantUI.SetActive(false);
        overviewUI.SetActive(false);
        scanningUI.SetActive(false);
    }
}
