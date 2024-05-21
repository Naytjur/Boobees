using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialMessage tutorialMessageStart;
    public TutorialMessage tutorialMessageStart2;
    public TutorialMessage tutorialMessagePlot;
    public TutorialMessage tutorialMessageUnlock;
    public TutorialMessage tutorialMessageScore;
    public TutorialMessage tutorialMessageLevel;

    private bool firstMessage = false;
    /*private bool plotPlaced = false;
    private bool secondMessage = false;
    private bool scoreTutorialShown = false;*/

    /*private void OnEnable()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
        ScoreManager.onScoreChanged += OnScoreChanged;
        ScoreManager.onLevelUp += OnLevelUp;
        PlantingManager.instance.onPlantUnlocked += OnPlantUnlocked;
    }


    private void OnDisable()
    {
        BuildManager.onBuildingPlaced -= OnBuildingPlaced;
        ScoreManager.onScoreChanged -= OnScoreChanged;
        ScoreManager.onLevelUp -= OnLevelUp;
        PlantingManager.instance.onPlantUnlocked -= OnPlantUnlocked;
    }*/

    private void Start()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
        ScoreManager.onScoreChanged += OnScoreChanged;
        ScoreManager.onLevelUp += OnLevelUp;
        PlantingManager.instance.onPlantUnlocked += OnPlantUnlocked;
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