using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    [SerializeField] private string id;

    public BuildingSO buildingSO;
    public BuildData buildData;

    [ContextMenu("Generate gui for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public static Building Create(Vector3 worldPosition, Vector2Int origin, BuildingSO buildingSO, BuildData data, float size, Quaternion rotation, BuildingSO.Dir dir)
    {
        Transform buildingTransform = Instantiate(buildingSO.gardenPrefab, worldPosition, rotation);

        Building building  = buildingTransform.GetComponent<Building>();
        building.buildingSO = buildingSO;
        building.buildData = data;
        building.buildData.gridX = origin.x;
        building.buildData.gridZ = origin.y;
        building.buildData.buildingRotation = (int) dir;


        buildingTransform.localScale = new Vector3(size, size, size);

        return building;
    }

    public void Move(Vector3 worldPosition, Vector2Int origin, Quaternion rotation, BuildingSO.Dir dir)
    {
        Show(); 
        transform.position = worldPosition;
        transform.rotation = rotation;

        buildData.gridX = origin.x;
        buildData.gridZ = origin.y;
        buildData.buildingRotation = (int) dir;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        Debug.Log("Hiding: " + gameObject.name);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnClick()
    {
        Debug.Log("Clicked: " + buildingSO.itemName);

        if(GameManager.instance.state == GameState.Building && BuildManager.instance.state == BuildManager.BuildState.Unselected)
        {
            BuildManager.instance.StartMoving(this);
        }
    }
}