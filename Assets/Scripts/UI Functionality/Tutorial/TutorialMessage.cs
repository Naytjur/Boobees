using UnityEngine;
using TMPro;

public class TutorialMessage : MonoBehaviour
{
    public GameObject tutorialUIPrefab; // Reference to the Tutorial UI prefab
    public string message; // Message to display in the text box

    private GameObject instantiatedUI;
    private TextMeshProUGUI textBox;
    private bool beenSeen;

    private void Awake()
    {
        // Instantiate the tutorial UI prefab
        instantiatedUI = Instantiate(tutorialUIPrefab);
        textBox = instantiatedUI.GetComponentInChildren<TextMeshProUGUI>();
        textBox.text = message;
        instantiatedUI.SetActive(false); // Start with the UI hidden

        // Add the TutorialUIClickHandler component and set it up
        TutorialUIClickHandler clickHandler = instantiatedUI.GetComponent<Canvas>().gameObject.AddComponent<TutorialUIClickHandler>();
        clickHandler.Setup(HideTutorialUI);
    }

    public void ShowTutorial()
    {
        if (instantiatedUI != null && !instantiatedUI.activeSelf && beenSeen != true)
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
}
