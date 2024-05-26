using System;
using UnityEngine;
using System.Collections.Generic;


public class TutorialManager : MonoBehaviour
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

        if (tutorialMessages.Count > 0)
        {
            tutorialMessages[0].ShowTutorial();
        }
    }

    private void Start()
    {
        tutorialMessageStart.ShowTutorial();
        firstMessage = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && firstMessage)
        {
            tutorialMessageStart2.ShowTutorial();
        }
    }

    public void LoadData(GameData data)
    {
        // Load seen tutorials
        List<bool> seenTutorials = data.seenTutorials;

        // Set seen tutorials
        for (int i = 0; i < tutorialMessages.Count; i++)
        {
            if (i < seenTutorials.Count)
            {
                tutorialMessages[i].beenSeen = seenTutorials[i];
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        // Save seen tutorials
        List<bool> seenTutorials = new List<bool>();
        foreach (var tutorialMessage in tutorialMessages)
        {
            seenTutorials.Add(tutorialMessage.HasBeenSeen());
        }
        data.seenTutorials = seenTutorials;
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
        if(level == 2)
        {
            tutorialMessageLevel.ShowTutorial();
        }
    }

    private void OnPlantUnlocked(PlantSO plant)
    {
        tutorialMessageUnlock.ShowTutorial();
    }
}