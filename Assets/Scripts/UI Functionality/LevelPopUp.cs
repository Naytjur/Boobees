using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelPopUp : MonoBehaviour
{
    [SerializeField]
    private ScoreManager scoreManager;
    [SerializeField]
    private TMP_Text levelUpText;

    private void Update()
    {
        levelUpText.text = "You have reached level " + scoreManager.playerLevel + " !";
    }

}
