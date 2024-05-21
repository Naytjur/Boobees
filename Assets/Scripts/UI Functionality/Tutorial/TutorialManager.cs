using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public TutorialMessage tutorialMessageStart;
    public TutorialMessage tutorialMessageStart2;
    public TutorialMessage tutorialMessagePlot;
    public TutorialMessage tutorialMessageScore;

    private bool plotPlaced = false;
    private bool firstMessage = false;
    private bool secondMessage = false;
    private bool scoreTutorialShown = false;

    private void OnEnable()
    {
        BuildManager.onBuildingPlaced += OnBuildingPlaced;
        ScoreManager.onScoreChanged += OnScoreChanged;
    }

    private void OnDisable()
    {
        BuildManager.onBuildingPlaced -= OnBuildingPlaced;
        ScoreManager.onScoreChanged -= OnScoreChanged;
    }

    private void Start()
    {
        if (!firstMessage)
        {
            tutorialMessageStart.ShowTutorial();
            firstMessage = true;
        }
    } 

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && firstMessage && !secondMessage)
        {
            tutorialMessageStart2.ShowTutorial();
            secondMessage = true;
        }
    }

    private void OnBuildingPlaced()
    {
        if (!plotPlaced)
        {
            plotPlaced = true;
            tutorialMessagePlot.ShowTutorial();
        }
    }

    private void OnScoreChanged(int pollen, int honey)
    {
        if (!scoreTutorialShown && (pollen > 0 || honey > 0))
        {
            tutorialMessageScore.ShowTutorial();
            scoreTutorialShown = true;
        }
    }
}