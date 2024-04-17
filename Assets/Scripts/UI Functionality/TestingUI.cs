using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    [SerializeField]
    private GameObject unlockButton;
    [SerializeField]
    private GameObject catalogueButton;

    private void Start()
    {
        GameManager.instance.onStateChange += ToggleExtraButton;
        GameManager.instance.onStateChange += ToggleTestingUI;
    }

    private void ToggleTestingUI(GameState state)
    {
        ui.SetActive(state != GameState.Catalogue);
    }

    private void ToggleExtraButton(GameState state)
    {
        unlockButton.SetActive(state == GameState.Viewing);
        catalogueButton.SetActive(state == GameState.Viewing);
    }
}
