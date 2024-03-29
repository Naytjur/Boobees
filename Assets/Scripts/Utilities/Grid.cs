using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridObject
{
    public Grid grid;

    public int x;
    public int z;

    public Building building;
}

public class Grid
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 origin;

    private GridObject[,] gridArray;


    public Grid(int width, int height, float cellSize, Vector3 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridArray = new GridObject[width, height];

        InitialiseArray();

        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    //Initialization
    private void InitialiseArray()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                gridArray[i, j].x = i;
                gridArray[i, j].z = j;
                gridArray[i, j].grid = this;
            }
        }
    }


    //Getters and Setters
    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + origin;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - origin).x * cellSize);
        y = Mathf.FloorToInt((worldPosition - origin).z * cellSize);
    }

    public void SetValue(int x, int z, Building building)
    {
        if(x  >= 0 && z >= 0 && x < width && z < height)
        {
            gridArray[x, z].building = building;
        }
    }

    public void SetValue(Vector3 worldPosition, Building building)
    {
        GetXZ(worldPosition, out int x, out int z);
        SetValue(x, z, building);

    }

    public GridObject GetGridObject(int x, int z)
    {
        return gridArray[x, z];
    }

    public GridObject GetGridObject(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return GetGridObject(x, z);
    }

    public bool IsPositionOnGrid(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            return true;
        }
        return false;
    }

    public bool IsPositionOnGrid(Vector3 worldPosition)
    {
        GetXZ(worldPosition, out int x, out int z);
        return IsPositionOnGrid(x, z);
    }
}
