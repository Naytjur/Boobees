using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class TutorialMessage : MonoBehaviour
{
    public GameObject tutorialUIPrefab; // Reference to the Tutorial UI prefab
    public LocalizedString message; // Message to display in the text box

    [SerializeField]
    public bool beenSeen = false;

    private GameObject instantiatedUI;
    private TextMeshProUGUI textBox;

    private LocalizeStringEvent localizeStringEvent; 

    private void Start()
    {
        instantiatedUI = Instantiate(tutorialUIPrefab);
        textBox = instantiatedUI.GetComponentInChildren<TextMeshProUGUI>();
        localizeStringEvent = instantiatedUI.GetComponent<TutorialMessageInfo>().localizeStringEvent;
        localizeStringEvent.StringReference = message;
        instantiatedUI.SetActive(false); // Start with the UI hidden

        // Add the TutorialUIClickHandler component and set it up
        TutorialUIClickHandler clickHandler = instantiatedUI.GetComponent<Canvas>().gameObject.AddComponent<TutorialUIClickHandler>();
        clickHandler.Setup(HideTutorialUI);
    }

    public void ShowTutorial()
    {
        if (instantiatedUI != null && !instantiatedUI.activeSelf && !beenSeen)
        {
            instantiatedUI.SetActive(true);
            beenSeen = true;
        }
    }

    private void HideTutorialUI()
    {
        if (instantiatedUI != null && instantiatedUI.activeSelf)
        {
            instantiatedUI.SetActive(false);
        }
    }

    public bool HasBeenSeen()
    {
        return beenSeen;
    }
}
