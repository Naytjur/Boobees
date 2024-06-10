using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableLevelUpButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        ScoreManager.onScoreChanged += UpdateButton;
        UpdateButton(0, 0);
    }

    private void UpdateButton(int honey, int pollen)
    {
        button.interactable = ScoreManager.instance.CanAffordLevel();
    }
}
