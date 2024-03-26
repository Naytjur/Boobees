using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBuilding : MonoBehaviour
{
    [Tooltip("index of chosen object in build manager's building array")]
    [SerializeField]
    private int index;

    [Tooltip("Not required if button is on the same gameobject")]
    [SerializeField]
    private Button button;

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
        buildManager.SetCurrentBuilding(index);
    }
}
