using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class LevelPopUpInfo : MonoBehaviour
{
    public TMP_Text levelUpText;
    public Transform unlockedImagesTransform;
    public int displayLevel;
    public LocalizeStringEvent unlockedLocalizedStringEvent;
    public GameObject unlockText;

    private void Awake()
    {
        displayLevel = ScoreManager.instance.playerLevel;
    }

}
