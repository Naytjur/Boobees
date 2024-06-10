using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;


public class ItemInfo : ScriptableObject
{
    public int id;
    public LocalizedString itemNameLocalizedString;
    public LocalizedString itemDescriptionLocalizedString;
    public string itemName;
    [TextArea(10, 20)]
    public string description;
    public bool unlocked;

    public Transform gardenPrefab;
    public Transform gardenVisual;
    public Transform cataloguePrefab;

    public Sprite sprite;
}
