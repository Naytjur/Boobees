using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantingManager : MonoBehaviour
{
    private bool isActive = false;

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Building;
    }
}
