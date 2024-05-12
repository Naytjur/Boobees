using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "New Insect", menuName = "Create Insect")]
public class InsectSO : ScriptableObject
{
    public Rarity rarity;
    public int pollenProduction;
    public int honeyProduction;

    public float insectFlySpeed = 1f;
    public float insectStayDuration = 6f;
    public float flyAwayDuration = 2f;
    public float rotationSpeed = 100f;
}
