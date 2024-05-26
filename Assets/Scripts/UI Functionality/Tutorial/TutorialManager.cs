using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour, IDataPersistence
{
    public TutorialMessage tutorialMessageStart;
    public TutorialMessage tutorialMessageStart2;
    public TutorialMessage tutorialMessagePlot;
    public TutorialMessage tutorialMessageUnlock;
    public TutorialMessage tutorialMessageScore;
    public TutorialMessage tutorialMessageLevel;

    private bool firstMessage = false;

    public List<TutorialMessage> tutorialMessages = new List<TutorialMessage>();

    private void Awake()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
        ScoreManager.onScoreChanged += OnScoreChanged;
        ScoreManager.onLevelUp += OnLevelUp;
        PlantingManager.instance.onPlantUnlocked += OnPlantUnlocked;

        TutorialMessage[] messages = FindObjectsOfType<TutorialMessage>();
        foreach (TutorialMessage message in messages)
        {
            tutorialMessages.Add(message);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && firstMessage)
        {
            tutorialMessageStart2.ShowTutorial();
            firstMessage = false; // Reset to avoid repeatedly showing the second tutorial
        }
    }

    public void LoadData(GameData data)
    {
        List<string> seenTutorials = data.seenTutorials;
        Debug.Log("Loading seen tutorials: " + string.Join(", ", seenTutorials));  // Debug log to check values

        for (int i = 0; i < tutorialMessages.Count; i++)
        {
            if (i < seenTutorials.Count)
            {
                tutorialMessages[i].beenSeen = seenTutorials[i];
            }
            Debug.Log($"Tutorial {i} beenSeen: {tutorialMessages[i].beenSeen}");
        }
        PlayFirstTutorial();
    }
    public void SaveData(ref GameData data)
    {
        List<string> seenTutorials = new List<string>();
        foreach (var tutorialMessage in tutorialMessages)
        {
            seenTutorials.Add(tutorialMessage.beenSeen);
        }
        data.seenTutorials = seenTutorials;
        Debug.Log("Saving seen tutorials: " + string.Join(", ", seenTutorials));  // Debug log to check values
    }

    private void OnBuildingPlaced()
    {
        tutorialMessagePlot.ShowTutorial();
    }

    private void OnScoreChanged(int pollen, int honey)
    {
        if ((pollen > 0 || honey > 0))
        {
            tutorialMessageScore.ShowTutorial();
        }
    }

    private void OnLevelUp(int level)
    {
        if (level == 2)
        {
            tutorialMessageLevel.ShowTutorial();
        }
    }

    private void OnPlantUnlocked(PlantSO plant)
    {
        tutorialMessageUnlock.ShowTutorial();
    }

    public void PlayFirstTutorial()
    {
        if (tutorialMessageStart.beenSeen == "false")
        {
            tutorialMessageStart.ShowTutorial();
            firstMessage = true; // This ensures the second message shows up correctly
        }
    }
}
