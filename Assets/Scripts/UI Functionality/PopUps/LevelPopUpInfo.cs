using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LevelPopUpInfo : MonoBehaviour
{
    public TMP_Text levelUpText;
    public Transform unlockedImagesTransform;
    public int displayLevel;

    private void Awake()
    {
        displayLevel = ScoreManager.instance.playerLevel;
    }

}
