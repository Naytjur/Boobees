using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] private float updateInterval = 5f;
    [SerializeField] private float scoreCapModifier = 2f;
    [SerializeField] private int maxHoneyScoreBase = 100;
    [SerializeField] private int maxPollenScoreBase = 100;
    [SerializeField] private int playerLevel = 1;

    public TMP_Text honeyText;
    public TMP_Text pollenText;
    public TMP_Text levelText;

    private int honeyScore = 0;
    private int pollenScore = 0;
    private int maxHoneyScore;
    private int maxPollenScore;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxHoneyScore = maxHoneyScoreBase;
        maxPollenScore = maxPollenScoreBase;

        honeyText.text = "Honey: " + honeyScore;
        pollenText.text = "Pollen: " + pollenScore;
        levelText.text = "Level: " + playerLevel;
    }

    public void UpdateScores(int pollen, int honey)
    {
        honeyScore += honey;
        pollenScore += pollen;

        honeyScore = Mathf.Clamp(honeyScore, 0, maxHoneyScore);

        if (pollenScore >= maxPollenScore)
        {
            pollenScore = 0;
            LevelUp();
        }

        honeyText.text = $"Honey: {honeyScore} / {maxHoneyScore}";
        pollenText.text = $"Pollen: {pollenScore} / {maxPollenScore}";
        levelText.text = "Level: " + playerLevel;
    }

    private void LevelUp()
    {
        playerLevel++;
        maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
        maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
    }

    private void Test(PlantSO plant)
    {
        Debug.Log(plant.name);
    }
}
