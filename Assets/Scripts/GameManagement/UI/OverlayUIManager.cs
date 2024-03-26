using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject[] UIs;

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateUI;
    }

    private void UpdateUI(GameState state)
    {
        switch (state)
        {
            case GameState.Viewing:
                SwitchTo(0);
                break;
            case GameState.Planting:
                SwitchTo(1);
                break;
            case GameState.Building:
                SwitchTo(2);
                break;
        }
    }

    private void SwitchTo(int index)
    {
        for (int i = 0; i < UIs.Length; i++)
        {
            UIs[i].SetActive(i == index);
        }
    }
}
