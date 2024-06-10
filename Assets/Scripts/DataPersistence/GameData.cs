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
    public float logoutTime;

    [SerializeField]
    public List<int> unlockedPlantIDs;
    [SerializeField]
    public List<int> unlockedInsectIDs;
    [SerializeField]
    public List<string> seenTutorials;
    [SerializeField]
    public List<BuildData> buildingList;
    [SerializeField]
    public List<PlantData> plantList;    

    public GameData()
    {
        this.playerLevel = 1;
        this.playerHoney = 0;
        this.playerPollen = 0;
        this.playerPollenCap = 0;
        this.playerHoneyCap = 0;
        this.logoutTime = 0;
        unlockedPlantIDs = new List<int>();
        unlockedInsectIDs = new List<int>();
        seenTutorials = new List<string>();
        buildingList = new List<BuildData>();
        plantList = new List<PlantData>();
    }
}
