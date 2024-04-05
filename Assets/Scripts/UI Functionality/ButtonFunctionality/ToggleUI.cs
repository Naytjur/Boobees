using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUI : MonoBehaviour
{
    [SerializeField]
    private GameObject uiElement;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Toggle);
    }

    public void Toggle()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(!uiElement.activeSelf);
        }
    }
}
