using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int playerLevel;
    public int playerHoney;
    public int playerPollen;

    public SerializableDictionary<string, bool> currentPatches;
    

    public GameData()
    {
        this.playerLevel = 0;
        this.playerHoney = 0;
        this.playerPollen = 0;

        currentPatches = new SerializableDictionary<string, bool>();

    }
}