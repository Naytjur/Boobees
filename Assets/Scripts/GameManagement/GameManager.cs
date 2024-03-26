using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Viewing,
    Planting,
    Building
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameState state { private set; get; }

    public event Action<GameState> onStateChange;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        state =  GameState.Viewing;
    }

    public void UpdateGameState(GameState newState)
    {
        if (state == newState)
        {
            return;
        }

        state = newState;

        switch (newState)
        {
            case GameState.Viewing:
                HandleViewing();
                break;
            case GameState.Planting:
                HandlePlanting();
                break;
            case GameState.Building:
                HandleBuilding();
                break;
        }

        onStateChange?.Invoke(newState);
    }

    private void HandleViewing()
    {
        Debug.Log("Now Viewing");
    }

    private void HandlePlanting()
    {
        Debug.Log("Now Planting");
    }

    private void HandleBuilding()
    {
        Debug.Log("Now Building");
    }
}
