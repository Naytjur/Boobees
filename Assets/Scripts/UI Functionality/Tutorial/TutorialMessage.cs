using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class TutorialMessage : MonoBehaviour
{
    public GameObject tutorialUIPrefab; // Reference to the Tutorial UI prefab
    public GameObject uiPanelPrefab;
    public LocalizedString message; // Message to display in the text box

    [SerializeField]
    public string beenSeen = "false";

    public bool useClickHandler = true; // Toggle for the type of tutorial
    public Button targetButton;

    private GameObject instantiatedUI;
    private GameObject instantiatedPanel;
    private TextMeshProUGUI textBox;

    private LocalizeStringEvent localizeStringEvent;

    public void ShowTutorial()
    {

        instantiatedUI = Instantiate(tutorialUIPrefab);
        textBox = instantiatedUI.GetComponentInChildren<TextMeshProUGUI>();
        localizeStringEvent = instantiatedUI.GetComponent<TutorialMessageInfo>().localizeStringEvent;
        localizeStringEvent.StringReference = message;
        instantiatedUI.SetActive(false); // Start with the UI hidden

        if (useClickHandler)
        {
            TutorialUIClickHandler clickHandler = instantiatedUI.GetComponent<Canvas>().gameObject.AddComponent<TutorialUIClickHandler>();
            clickHandler.Setup(HideTutorialUI);
        }
        else
        {
            Vector3 buttonPos = targetButton.transform.position;

            // Instantiate the UI panel independently
            instantiatedPanel = Instantiate(uiPanelPrefab);
            instantiatedPanel.transform.position = buttonPos;

            // Ensure the panel is rendered behind the UI
            instantiatedPanel.GetComponent<Canvas>().sortingOrder = instantiatedUI.GetComponent<Canvas>().sortingOrder - 1;

            // Allow clicks to pass through
            CanvasGroup canvasGroup = instantiatedPanel.AddComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;

            Debug.Log("UI panel instantiated");
        }

        if (instantiatedUI != null && !instantiatedUI.activeSelf && beenSeen == "false")
        {
            instantiatedUI.SetActive(true);
            beenSeen = "true";
            Debug.Log($"Showing tutorial: {message}, beenSeen set to true");
        }
    }

    public void HideTutorialUI()
    {
        if (instantiatedUI != null && instantiatedUI.activeSelf)
        {
            instantiatedUI.SetActive(false);
            Destroy(instantiatedPanel);
        }
    }

    public string HasBeenSeen()
    {
        return beenSeen;
    }
}
