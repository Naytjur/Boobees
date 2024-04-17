using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectBuilding : MonoBehaviour
{
    [Tooltip("index of chosen object in build manager's building array")]
    [SerializeField]
    private int index;

    [Tooltip("Not required if button is on the same gameobject")]
    public Button button;
    public TMP_Text buildingNameText;
    public TMP_Text buildingAmountText;

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
        buildManager.SetBuildingByIndex(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
}
