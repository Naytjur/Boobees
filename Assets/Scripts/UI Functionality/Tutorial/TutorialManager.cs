using System;
using UnityEngine;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour, IDataPersistence
{
    public TutorialMessage tutorialMessageLanguage;
    public TutorialMessage tutorialMessageStart;
    public TutorialMessage tutorialMessageStart2;
    public TutorialMessage tutorialMessagePlot;
    public TutorialMessage tutorialMessageUnlock;
    public TutorialMessage tutorialMessageScore;
    public TutorialMessage tutorialMessageLevel;

    public List<TutorialMessage> tutorialMessages = new List<TutorialMessage>();

    public LanguageManager languageManager;

    private bool postLoadCompleted = false;
    public GameObject blackOutScreen;

    private void Awake()
    {
        // Temporarily disable event subscription for onBuildingPlaced
        postLoadCompleted = false;

        TutorialMessage[] messages = FindObjectsOfType<TutorialMessage>();
        blackOutScreen.SetActive(false);

        foreach (TutorialMessage message in messages)
        {
            tutorialMessages.Add(message);
            message.manager = this;
        }
    }
    
    void Start()
    {
        ScoreManager.onScoreChanged += OnScoreChanged;
        ScoreManager.onLevelUp += OnLevelUp;
        DataPersistenceManager.postLoad += PostLoad;
        PlantingManager.instance.onPlantUnlocked += OnPlantUnlocked;
    }

    private void OnEnable()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
    }

    private void OnDisable()
    {
        BuildManager.onBuildingPlaced -= OnBuildingPlaced;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (tutorialMessageStart2.beenSeen == "false" && tutorialMessageStart.beenSeen == "true")
            {
                tutorialMessageStart2.ShowTutorial();
            }
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

    private void PostLoad()
    {
        LanguageTutorial();

        // Enable the flag to allow OnBuildingPlaced to execute
        postLoadCompleted = true;
    }

    private void OnBuildingPlaced()
    {
        if (postLoadCompleted)
        {
            tutorialMessagePlot.ShowTutorial();
        }
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
        if (tutorialMessageStart.beenSeen != "true")
        {
            Debug.Log("PeeperSweeper");
            tutorialMessageStart.ShowTutorial();
        }
    }

    public void LanguageTutorial()
    {
        if (tutorialMessageLanguage.beenSeen == "false")
        {
            tutorialMessageLanguage.ShowTutorial();
        }
    }
        public void HideBlackoutPanel()
    {
        blackOutScreen.SetActive(false);
    }
}
