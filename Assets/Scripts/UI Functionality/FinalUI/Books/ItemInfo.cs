using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : ScriptableObject
{
    public string id;
    public string itemName;
    [TextArea(10, 20)]
    public string description;
    public bool unlocked;

    public Transform prefab;
    public Transform visual;

    public Sprite sprite;
}
