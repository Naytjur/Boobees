using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerLevel;
    public int playerHoney;
    public int playerPollen;
    public int playerHoneyCap;
    public int playerPollenCap;

    [SerializeField]
    public List<string> unlockedPlantIDs;
    
    public GameData()
    {
        this.playerLevel = 0;
        this.playerHoney = 0;
        this.playerPollen = 0;
        this.playerPollenCap = 0;
        this.playerHoneyCap = 0;
        unlockedPlantIDs = new List<string>();
    }
}