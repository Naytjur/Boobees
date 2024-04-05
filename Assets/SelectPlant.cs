using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlant : MonoBehaviour
{
    [Tooltip("index of chosen object in build manager's building array")]
    [SerializeField]
    private int index;

    [Tooltip("Not required if button is on the same gameobject")]
    [SerializeField]
    private Button button;

    private PlantingManager plantingManager;

    private void Start()
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
        plantingManager.SwitchPlant(index);
    }

    public void SetIndex(int index)
    {
        this.index = index;
    }
}
