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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isActive)
        {
            HandleClick();
        }
    }

    private void HandleClick()
    {
        if (Mouse3D.GetMouseWorldPosition(LayerMask.GetMask("BuildSurface"), out Vector3 pos))
        {
            Grid grid = BuildManager.instance.buildGrid;
            grid.GetXZ(pos, out int x, out int z);

            if (grid.IsPositionOnGrid(x, z) && grid.GetGridObject(x, z).building != null)
            {
                grid.GetGridObject(x, z).building.OnClick();
            }
        }
    }

    //GameFlow
    private void UpdateActiveState(GameState state)
    {
        isActive = state == GameState.Viewing;
        Debug.Log(state);
    }
}
