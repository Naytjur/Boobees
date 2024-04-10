using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InsectManager : MonoBehaviour
{
    [SerializeField] private float updateInterval = 5f;
    [SerializeField] private float insectFlySpeed = 1f;
    [SerializeField] private float insectStayDuration = 6f;
    [SerializeField] private float scoreCapModifier = 2f;
    [SerializeField] private float flyAwayDuration = 2f;
    [SerializeField] private float RotationSpeed = 100f;

    [SerializeField] private int maxHoneyScoreBase = 100;
    [SerializeField] private int maxPollenScoreBase = 100;
    [SerializeField] private int playerLevel = 1;

    public TMP_Text honeyText;
    public TMP_Text pollenText;
    public TMP_Text levelText;

    public List<GameObject> allInsects = new List<GameObject>();
    public List<Plant> allPlants = new List<Plant>();

    private int honeyScore = 0;
    private int pollenScore = 0;
    private int maxHoneyScore;
    private int maxPollenScore;

    private void Start()
    {
        maxHoneyScore = maxHoneyScoreBase;
        maxPollenScore = maxPollenScoreBase;

        honeyText.text = "Honey: " + honeyScore;
        pollenText.text = "Pollen: " + pollenScore;
        levelText.text = "Level: " + playerLevel;

        InvokeRepeating(nameof(UpdateScores), updateInterval, updateInterval);

        PlantingManager.instance.PlantPlanted += OnPlantPlanted;
    }

    private void UpdateScores()
    {
        if (allInsects.Count == 0 || allPlants.Count == 0)
            return;

        int honeyGain = 0;
        int pollenGain = 0;

        foreach (Plant plant in allPlants)
        {
            foreach (GameObject insectPrefab in plant.insects)
            {
                Insect insectScript = insectPrefab.GetComponent<Insect>();
                if (insectScript != null && Random.Range(1, 101) <= insectScript.Rarity * 10)
                {
                    honeyGain += insectScript.HoneyP;
                    pollenGain += insectScript.PollenP;

                    StartCoroutine(MoveInsectToPlant(insectPrefab, plant.transform.position));
                }
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

    private IEnumerator MoveInsectToPlant(GameObject insectPrefab, Vector3 targetPosition)
    {
        Vector3 spawnPosition = targetPosition + Vector3.up * Random.Range(1f, 3f) +
                                Random.insideUnitSphere * 2f;
        GameObject newInsect = Instantiate(insectPrefab, spawnPosition, Quaternion.identity);

        Transform insectTransform = newInsect.transform;

        Vector3 initialPosition = insectTransform.position;
        float journeyLength = Vector3.Distance(spawnPosition, targetPosition);
        float startTime = Time.time;

        while ((Time.time - startTime) < insectStayDuration)
        {
            float distanceCovered = (Time.time - startTime) * insectFlySpeed;
            float journeyFraction = distanceCovered / journeyLength;
            insectTransform.position = Vector3.Lerp(spawnPosition, targetPosition, journeyFraction);

            Vector3 direction = (targetPosition - insectTransform.position).normalized;

            Quaternion targetRotation;
            if (direction != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(direction);
                insectTransform.rotation = Quaternion.Lerp(insectTransform.rotation, targetRotation, Time.deltaTime * RotationSpeed);
            }

            yield return null;
        }

        Quaternion flyAwayRotation = Quaternion.LookRotation(spawnPosition - targetPosition);
        float flyAwayStartTime = Time.time;
        while ((Time.time - flyAwayStartTime) < flyAwayDuration)
        {
            insectTransform.rotation = Quaternion.Lerp(insectTransform.rotation, flyAwayRotation, Time.deltaTime * RotationSpeed);
            insectTransform.Translate(Vector3.forward * insectFlySpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(newInsect);
    }

    private void OnPlantPlanted()
    {
        allPlants.Clear();
        allInsects.Clear();
        foreach (Plant plant in PlantingManager.instance.plantList)
        {
            allPlants.Add(plant);
            foreach (GameObject insectPrefab in plant.insects)
            {
                allInsects.Add(insectPrefab);
            }
        }
    }
    private void LevelUp()
    {
        playerLevel++;
        maxHoneyScore = Mathf.RoundToInt(maxHoneyScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
        maxPollenScore = Mathf.RoundToInt(maxPollenScoreBase * Mathf.Pow(scoreCapModifier, playerLevel));
    }
}
