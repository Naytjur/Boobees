using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnlockPlant : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private TMP_Text confirmInputText;

    [SerializeField]
    private PlantSO[] plants;

    private void Start()
    {
        confirmButton.onClick.AddListener(ConfirmInput);
    }

    private void ConfirmInput()
    {
        if(int.TryParse(inputField.text, out int number))
        {
            if(PlantingManager.instance.TryUnlockPlant(number, out string name))
            {
                confirmInputText.text = name;
                gameObject.SetActive(false);
                return;
            }
        }
        confirmInputText.text = "Invalid code!";
    }
}
