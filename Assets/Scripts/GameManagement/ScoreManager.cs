using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;

    [SerializeField] private float scoreCapModifier = 2f;
    [SerializeField] public int maxHoneyScoreBase = 20;
    [SerializeField] public int maxPollenScoreBase = 50;
    [SerializeField] public int playerLevel = 1;

    public TMP_Text levelText;

    public int honeyScore = 0;
    public int pollenScore = 0;
    public int maxHoneyScore = 20;
    public int maxPollenScore = 50;
    public float downtimeScoreModifier = 0.01f;

    public float logoutTime;



    public static event Action<int> onLevelUp;
    public static event Action<int, int> onScoreChanged;

    public List<PlantSO> allPlants;

    private bool isLoading = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        DataPersistenceManager.postLoad += OnPostLoad;
    }

    private void Start()
    {
        maxHoneyScore = maxHoneyScoreBase;
        maxPollenScore = maxPollenScoreBase;
    }

    public void LoadData(GameData data)
    {
        isLoading = true; // Set the flag to true at the start of loading

        this.playerLevel = data.playerLevel;
        this.honeyScore = data.playerHoney;
        this.pollenScore = data.playerPollen;
        this.maxHoneyScore = data.playerHoneyCap;
        this.maxPollenScore = data.playerPollenCap;
        this.logoutTime = data.logoutTime;

        foreach (string plantID in data.unlockedPlantIDs)
        {
            PlantSO plant = FindPlantByID(plantID);
            if (plant != null)
            {
                plant.unlocked = true;
            }
        }

        Debug.Log("Loading GameData");

        isLoading = false; // Reset the flag after loading is done
    }

    public void SaveData(ref GameData data)
    {
        data.logoutTime = GetCurrentDateTimeAsFloat();
        data.playerLevel = this.playerLevel;
        data.playerHoney = this.honeyScore;
        data.playerPollen = this.pollenScore;
    }

    private void OnPostLoad()
    {
        maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel - 1));
        maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
        DowntimeGains();
        UpdateUI();
    }

    private void UpdateUI()
    {
        levelText.text = playerLevel.ToString();
    }

    public void UpdateScores(int pollen, int honey)
    {
        honeyScore += honey;
        pollenScore += pollen;

        honeyScore = Mathf.Clamp(honeyScore, 0, maxHoneyScore);
        UpdateUI();
        onScoreChanged?.Invoke(pollen, honey);
    }

    public void LevelUp()
    {
        if (CanAffordLevel())
        {
            playerLevel++;
            honeyScore = 0;
            pollenScore = pollenScore - maxPollenScore;
            maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel - 1));
            maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
            onLevelUp?.Invoke(playerLevel);
            UpdateUI(); // Update level text
        }
    }

    private PlantSO FindPlantByID(string id)
    {
        foreach (PlantSO plant in allPlants)
        {
            if (plant.id == id)
            {
                return plant;
            }
        }
        return null;
    }

    public bool CanAffordBuy(int cost)
    {
        return cost <= honeyScore;
    }

    public bool CanAffordLevel()
    {
        return pollenScore >= maxPollenScore && honeyScore == maxHoneyScore;
    }

    public static float GetCurrentDateTimeAsFloat()
    {
        // Get the current date and time
        DateTime now = DateTime.UtcNow;

        // Define the Unix epoch (January 1, 1970)
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Calculate the difference between now and the Unix epoch
        TimeSpan timeSpan = now - epoch;

        // Return the total seconds as a float
        return (float)timeSpan.TotalSeconds;
    }

    private void DowntimeGains()
    {
        if (logoutTime != 0)
        {
            // Calculate downtime
            float currentTime = GetCurrentDateTimeAsFloat();
            float downtime = currentTime - logoutTime;

            // Calculate additional scores
            int additionalPollen = Mathf.FloorToInt(downtime * downtimeScoreModifier * playerLevel);
            int additionalHoney = Mathf.FloorToInt(downtime  * downtimeScoreModifier * playerLevel);

            // Update scores
            pollenScore += additionalPollen;
            honeyScore += additionalHoney;

            // Ensure honey does not exceed its cap
            honeyScore = Mathf.Clamp(honeyScore, 0, maxHoneyScore);

            // Update the UI with the new scores
            onScoreChanged?.Invoke(additionalPollen, additionalHoney);
        }
    }
}