using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private BuildingSO buildingSO;
    private Vector2Int origin;

    public static Building Create(Vector3 worldPosition, Vector2Int origin, BuildingSO buildingSO, float size, Quaternion rotation)
    {
        Transform buildingTransform = Instantiate(buildingSO.prefab, worldPosition, rotation);

        Building building  = buildingTransform.GetComponent<Building>();

        building.buildingSO = buildingSO;
        building.origin = origin;

        buildingTransform.localScale = new Vector3(size, size, size);

        return building;
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked: " + buildingSO.buildingName);
    }
}
