using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAR : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ToggleARFunction);
    }

    private void ToggleARFunction()
    {
        SceneManager.instance.ToggleAR();
        Debug.Log("Toggled");
    }
}
