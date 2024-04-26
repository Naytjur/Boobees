using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance;

    [SerializeField]
    private GameObject arScene;
    [SerializeField]
    private GameObject mainScene;

    private void Start()
    {
        instance = this;
    }

    public void ToggleAR()
    {
        if (arScene != null)
        {
            arScene.SetActive(!arScene.activeSelf);
            mainScene.SetActive(!mainScene.activeSelf);
        }
    }
}
