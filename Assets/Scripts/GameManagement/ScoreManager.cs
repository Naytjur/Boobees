using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class ScoreManager : MonoBehaviour, IDataPersistence
{
    public static ScoreManager instance;

    [SerializeField] private float scoreCapModifier = 1.6f;
    [SerializeField] private int maxHoneyScoreBase = 100;
    [SerializeField] private int maxPollenScoreBase = 20;
    [SerializeField] public int playerLevel = 1;

    public TMP_Text honeyText;
    public TMP_Text pollenText;
    public TMP_Text levelText;

    private int honeyScore = 0;
    private int pollenScore = 0;
    private int maxHoneyScore;
    private int maxPollenScore;

    public static event Action<int> onLevelUp;
    public static event Action<int, int> onScoreChanged;

    public List<PlantSO> allPlants;

    private bool isLoading = false;

    private void Awake()
    {
        instance = this;
<<<<<<< HEAD
=======
    }

    private void OnEnable()
    {
        DataPersistenceManager.postLoad += OnPostLoad;
    }

    private void Start()
    {
        maxHoneyScore = maxHoneyScoreBase;
        maxPollenScore = maxPollenScoreBase;
>>>>>>> Luiz_Adding_Insects
    }

    public void LoadData(GameData data)
    {
        isLoading = true; // Set the flag to true at the start of loading

        this.playerLevel = data.playerLevel;
        this.honeyScore = data.playerHoney;
        this.pollenScore = data.playerPollen;
        this.maxHoneyScore = data.playerHoneyCap;
        this.maxPollenScore = data.playerPollenCap;

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
        data.playerLevel = this.playerLevel;
        data.playerHoney = this.honeyScore;
        data.playerPollen = this.pollenScore;
    }

    private void OnPostLoad()
    {
        maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
        maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel - 1));
        UpdateUI();
    }

    private void UpdateUI()
    {
        honeyText.text = $"{honeyScore} / {maxHoneyScore}";
        pollenText.text = $"{pollenScore} / {maxPollenScore}";
        levelText.text = playerLevel.ToString();
    }

    public void UpdateScores(int pollen, int honey)
    {
        honeyScore += honey;
        pollenScore += pollen;

        honeyScore = Mathf.Clamp(honeyScore, 0, maxHoneyScore);

        if (pollenScore >= maxPollenScore)
        {
            LevelUp();
        }

        UpdateUI();
        onScoreChanged?.Invoke(pollen, honey);
    }

    private void LevelUp()
    {
        playerLevel++;
        maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
        maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel - 1));
        onLevelUp?.Invoke(playerLevel);
        UpdateUI(); // Update level text
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

    public bool CanAfford(int cost)
    {
        return cost <= honeyScore;
    }
}