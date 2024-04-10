using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsectManager : MonoBehaviour
{
    public List<GameObject> allInsects = new List<GameObject>();
    public List<Plant> allPlants = new List<Plant>();

    [SerializeField] private int maxHoneyScoreBase = 100;
    [SerializeField] private int maxPollenScoreBase = 100;
    [SerializeField] private float scoreCapModifier = 1.5f;

    private int honeyScore = 0;
    private int pollenScore = 0;
    private int maxHoneyScore;
    private int maxPollenScore;
    private int playerLevel = 1;

    public TMP_Text honeyText;
    public TMP_Text pollenText;
    public TMP_Text levelText;

    private float updateInterval = 1f;

    private void Start()
    {
        CreateCombinedLists();
        maxHoneyScore = maxHoneyScoreBase;
        maxPollenScore = maxPollenScoreBase;

        honeyText.text = "Honey: " + honeyScore;
        pollenText.text = "Pollen: " + pollenScore;
        levelText.text = "Level: " + playerLevel;

        InvokeRepeating(nameof(UpdateScores), updateInterval, updateInterval);
    }

    private void UpdateScores()
    {
        if (allInsects.Count == 0)
            return;

        int honeyGain = 0;
        int pollenGain = 0;

        foreach (GameObject insect in allInsects)
        {
            Insect insectScript = insect.GetComponent<Insect>();
            if (insectScript != null)
            {
                honeyGain += insectScript.HoneyP;
                pollenGain += insectScript.PollenP;
            }
        }

        honeyScore += honeyGain;
        pollenScore += pollenGain;

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
    public void UpdateLists(Plant newPlant)
    {
        allInsects.AddRange(newPlant.insects);
        allPlants.Add(newPlant);
    }

    private void CreateCombinedLists()
    {
        List<Plant> plantList = new List<Plant>(FindObjectsOfType<Plant>());
        UpdateInsectList(plantList);
    }

    private void UpdateInsectList(IEnumerable<Plant> plantList)
    {
        allInsects.Clear();

        foreach (Plant plantScript in plantList)
        {
            foreach (GameObject insect in plantScript.insects)
            {
                allInsects.Add(insect);
            }
        }
    }
}
