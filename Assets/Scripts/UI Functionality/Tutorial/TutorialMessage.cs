using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TutorialMessage : MonoBehaviour
{
    public GameObject tutorialUIPrefab; // Reference to the Tutorial UI prefab
    public GameObject blackOutScreen;
    public LocalizedString message; // Message to display in the text box
    public TutorialManager manager;

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

        localizeStringEvent = instantiatedUI.GetComponent<TutorialMessageInfo>()?.localizeStringEvent;
        if(localizeStringEvent != null)
        {
            localizeStringEvent.StringReference = message;
        }

        instantiatedUI.SetActive(false); // Start with the UI hidden

        // Add the TutorialUIClickHandler component and set it up
        if (useClickHandler)
        {
            TutorialUIClickHandler clickHandler = instantiatedUI.GetComponent<TutorialUIClickHandler>();
            if(clickHandler != null)
            {
                clickHandler.Setup(HideTutorialUI);
            }

            LanguagePopUp languages = instantiatedUI.GetComponent<LanguagePopUp>();

            if(languages != null)
            {
                for(int i = 0; i < languages.languageButtons.Length; i++)
                {
                    languages.languageButtons[i].onClick.AddListener(HideTutorialUI);
                    languages.languageButtons[i].onClick.AddListener(manager.PlayFirstTutorial);

                    switch (i)
                    {
                        case 0:
                            languages.languageButtons[i].onClick.AddListener(delegate { manager.languageManager.ChangeLanguage(0); });
                            break;
                        case 1:
                            languages.languageButtons[i].onClick.AddListener(delegate { manager.languageManager.ChangeLanguage(1); });
                            break;
                        case 2:
                            languages.languageButtons[i].onClick.AddListener(delegate { manager.languageManager.ChangeLanguage(2); });
                            break;
                        default:
                            languages.languageButtons[i].onClick.AddListener(delegate { manager.languageManager.ChangeLanguage(1); });
                            break;
                    }
                }
            }
        }
        else
        {
            Vector3 buttonPos = targetButton.transform.position;

            // Instantiate the UI panel independently
            blackOutScreen.SetActive(true);
            blackOutScreen.transform.position = buttonPos;
            // Allow clicks to pass through
            //blackOutScreen.blocksRaycasts = false;

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
            manager.HideBlackoutPanel();
        }
    }
    public string HasBeenSeen()
    {
        return beenSeen;
    }
}
