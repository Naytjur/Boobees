using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildVisual : MonoBehaviour
{
    private Transform visual;
    private BuildingSO buildingSO;

    private bool hasPositionOnGrid = false;
    private Vector3 lastTarget;

    private float scale;

    private void Start()
    {
        BuildManager.onBuildingChanged += ChangeVisual;
        GameManager.instance.onStateChange += Toggle;
    }

    private void LateUpdate()
    {
        if(visual != null)
        {
            UpdateVisual();
        }
    }

    private void UpdateVisual()
    {
        if(BuildManager.instance.GetMouseGridPosition(out Vector3 hit))
        {
            lastTarget = hit;

            if(!hasPositionOnGrid)
            {
                visual.transform.position = hit;
                hasPositionOnGrid = true;
            }
        }

        visual.transform.position = Vector3.Lerp(visual.transform.position, lastTarget, Time.deltaTime * 20f);
    }

    private void ChangeVisual()
    {
        hasPositionOnGrid = false;

        if(visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }
        
        BuildingSO buildingObject = BuildManager.instance.GetCurrentBuilding();

        if(buildingObject != null && GameManager.instance.state == GameState.Building)
        {
            visual = Instantiate(buildingObject.visual, new Vector3(-1000, -1000, -1000), Quaternion.identity);
            visual.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void Toggle(GameState state)
    {
        ChangeVisual();
    }
}
