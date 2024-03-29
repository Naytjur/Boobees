using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingSO buildingSO;
    private Vector2Int origin;

    public static Building Create(Vector3 worldPosition, Vector2Int origin, BuildingSO buildingSO)
    {
        Transform buildingTransform = Instantiate(buildingSO.prefab, worldPosition, Quaternion.identity);

        Building building  = buildingTransform.GetComponent<Building>();

        building.buildingSO = buildingSO;
        building.origin = origin;

        return building;
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked: " + buildingSO.buildingName);
    }
}
