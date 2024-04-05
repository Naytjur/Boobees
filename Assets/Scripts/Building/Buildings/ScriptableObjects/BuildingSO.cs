using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Create Building")]
public class BuildingSO : ScriptableObject
{
    public string buildingName;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;

    public enum Dir
    {
        Front,
        Left,
        Back,
        Right
    }

    public List<Vector2Int> GetGridPositions(Vector2Int offset)
    {
        List<Vector2Int> gridPositionsList = new List<Vector2Int>();

        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                gridPositionsList.Add(offset + new Vector2Int(i, j));
            }
        }

        return gridPositionsList;
    }
}
