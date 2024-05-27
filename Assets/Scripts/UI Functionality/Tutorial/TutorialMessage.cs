using UnityEngine;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class TutorialMessage : MonoBehaviour
{
    public GameObject tutorialUIPrefab; // Reference to the Tutorial UI prefab
    public LocalizedString message; // Message to display in the text box

    [SerializeField]
    public string beenSeen = "false";

    private GameObject instantiatedUI;
    private TextMeshProUGUI textBox;

    private LocalizeStringEvent localizeStringEvent;

    private void Start()
    {
        
    }

    public void ShowTutorial()
    {

        instantiatedUI = Instantiate(tutorialUIPrefab);
        textBox = instantiatedUI.GetComponentInChildren<TextMeshProUGUI>();
        localizeStringEvent = instantiatedUI.GetComponent<TutorialMessageInfo>().localizeStringEvent;
        localizeStringEvent.StringReference = message;
        instantiatedUI.SetActive(false); // Start with the UI hidden

        // Add the TutorialUIClickHandler component and set it up
        TutorialUIClickHandler clickHandler = instantiatedUI.GetComponent<Canvas>().gameObject.AddComponent<TutorialUIClickHandler>();
        clickHandler.Setup(HideTutorialUI);

        if (instantiatedUI != null && !instantiatedUI.activeSelf && beenSeen == "false")
        {
            instantiatedUI.SetActive(true);
            beenSeen = "true";
            Debug.Log($"Showing tutorial: {message}, beenSeen set to true");
        }
    }

    private void HideTutorialUI()
    {
        if (instantiatedUI != null && instantiatedUI.activeSelf)
        {
            instantiatedUI.SetActive(false);
        }
    }

    public string HasBeenSeen()
    {
        return beenSeen;
    }
}
