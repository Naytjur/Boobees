using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class SelectPlant : MonoBehaviour
{
    [Tooltip("index of chosen object in build manager's building array")]
    [SerializeField]
    private int index;

    [Tooltip("Not required if button is on the same gameobject")]
    public Button button;
    public Image image;
    public TMP_Text text;

    private PlantingManager plantingManager;

    public LocalizeStringEvent plantNameLocalizeStringEvent;

    public void Setup()
    {
        plantingManager = Object.FindObjectOfType<PlantingManager>();

        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(delegate { Select(index); });
    }

    private void Select(int index)
    {
        Debug.Log(index);
        plantingManager.SwitchPlant(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }

    public int GetIndex()
    {
        return index;
    }
}
