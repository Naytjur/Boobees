using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;

public class ItemInfo : ScriptableObject
{
    public string id;
    public LocalizeStringEvent itemNameStringEvent;
    public string itemName;
    [TextArea(10, 20)]
    public string description;
    public bool unlocked;

    public Transform gardenPrefab;
    public Transform gardenVisual;
    public Transform cataloguePrefab;

    public Sprite sprite;
}
