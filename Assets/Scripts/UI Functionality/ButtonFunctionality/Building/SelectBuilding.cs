using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;

public class SelectBuilding : MonoBehaviour
{
    [Tooltip("index of chosen object in build manager's building array")]
    [SerializeField]
    private int index;

    [Tooltip("Not required if button is on the same gameobject")]
    public Button button;
    //public TMP_Text buildingNameText;
    public LocalizeStringEvent buildNameLocalizeStringEvent;
    public LocalizeStringEvent buildCostLocalizationEvent;
    public LocalizeStringEvent buildUnlockLocalizationEvent;
    public Image image;
    public TMP_Text buildingAmountText;
    public int buildingCost;
    public int buildingUnlockLevel;

    public BuildingSO building;

    private BuildManager buildManager;
    private void Start()
    {
        buildManager = Object.FindObjectOfType<BuildManager>();

        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(delegate { Select(index); });
    }

    private void Select(int index)
    {
        ScoreManager.instance.UpdateScores(0, -building.cost);
        buildManager.StartPlacing(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
}
