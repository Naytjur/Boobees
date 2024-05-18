using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "New Building", menuName = "Create Building")]
public class BuildingSO : ItemInfo
{
    //Luiz Garbage
    // private ShopItem item;
    // [ReadOnly()] public PlaceableObjectData data = new PlaceableObjectData();


    //Luiz Garbage
    public int width;
    public int height;

    public int unlockLevel;

    public int maxCount { get; private set; }
    public int count { get; private set; }

    public enum Dir
    {
        Down,
        Left,
        Up,
        Right
    }

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default: 
            case Dir.Down:
                return Dir.Left;
            case Dir.Left:
                return Dir.Up;
            case Dir.Up:
                return Dir.Right;
            case Dir.Right:
                return Dir.Down;
        }
    }

    public int GetRotationDegrees(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return 0;
            case Dir.Left:
                return 90;
            case Dir.Up:
                return 180;
            case Dir.Right:
                return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Down:
                return new Vector2Int(0, 0);
            case Dir.Left:
                return new Vector2Int(0, width);
            case Dir.Up:
                return new Vector2Int(width, height);
            case Dir.Right:
                return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositions(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionsList = new List<Vector2Int>();

        switch (dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        gridPositionsList.Add(offset + new Vector2Int(i, j));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for(int i = 0; i < height; i++)
                {
                    for(int j = 0; j < width; j++)
                    {
                        gridPositionsList.Add(offset + new Vector2Int(i, j));
                    }
                }
                break;
        }

        return gridPositionsList;
    }

    public void TryUnlock(int level)
    {
        if(level == unlockLevel)
        {
            Unlock();
        }
    }

    private void Unlock()
    {
        unlocked = true;
    }

    public void AddCount(int amount)
    {
        count += amount;
    }

    public void RemoveCount(int amount)
    {
        count -= amount;
    }

    public bool HasCountLeft()
    {
        return count < maxCount;
    }

    public void TryAdjustMaxCount(int level)
    {
        maxCount = Mathf.Max(0, level - unlockLevel);
    }
    //More Luiz Garbage
    // public void Initialize(ShopItem shopItem)
    // {
    //     item = shopItem;
    //     data.assetName = item.Name;
    //     data.ID = SaveData.GenerateId();
    // }

    // public void Initialize(ShopItem shopItem, PlaceableObjectData objectData)
    // {
    //     item = shopItem;
    //     data = objectData;
    // }

    // private void OnApplicationQuit()
    // {
    //     data.position = transform.postion;

    // }
}