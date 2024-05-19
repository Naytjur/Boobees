using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewManager : MonoBehaviour
{
    private bool isActive;

    private void Start()
    {
        GameManager.instance.onStateChange += UpdateActiveState;
        UpdateActiveState(GameManager.instance.state);
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Viewing;
        Debug.Log(state);
    }
}
