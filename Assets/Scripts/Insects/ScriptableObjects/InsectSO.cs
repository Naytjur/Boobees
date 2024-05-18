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
public class InsectSO : CatalogueItemInfo
{
    public enum Type
    {
        Bee,
        Butterfly
    }

    public Type type;
    public Rarity rarity;
    public int pollenProduction;
    public int honeyProduction;

    public float insectFlySpeed = 1f;
    public float insectStayDuration = 6f;
    public float flyAwayDuration = 2f;
    public float rotationSpeed = 100f;

    public int GetRarityPercentage()
    {
        int percentage = 0;
        switch (rarity)
        {
            case Rarity.Common:
                percentage = 20;
                break;
            case Rarity.Uncommon:
                percentage = 10;
                break;
            case Rarity.Rare:
                percentage = 5;
                break;
            case Rarity.Epic:
                percentage = 1;
                break;
        }

        return percentage;
    }
}
