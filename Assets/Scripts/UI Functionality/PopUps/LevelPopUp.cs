using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;

public class LevelPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpPrefab;
    [SerializeField]
    private GameObject popUpCanvas;
    [SerializeField]
    private GameObject unlockedImagePrefab;

    public LocalizedString unlockedLocalizedString;

    private bool isInitialized = false;

    private void OnEnable()
    {
        ScoreManager.onLevelUp += ShowLevelPopUp;
        DataPersistenceManager.postLoad += OnGameLoaded;
    }

    private void OnDisable()
    {
        ScoreManager.onLevelUp -= ShowLevelPopUp;
        DataPersistenceManager.postLoad -= OnGameLoaded;
    }

    private void ShowLevelPopUp(int level)
    {
        if (!isInitialized || level <= 1)
        {
            return;
        }

        GameObject popUp = Instantiate(popUpPrefab, popUpCanvas.transform);
        LevelPopUpInfo popUpInfo = popUp.GetComponent<LevelPopUpInfo>();
        var unlockText = popUpInfo.unlockText;
        bool unlockedItems = false;

        foreach (BuildingSO building in BuildManager.instance.GetBuildingList())
        {
            if (building.unlockLevel == level)
            {
                ShowImage(popUpInfo.unlockedImagesTransform, building.sprite);
                unlockedItems = true;
            }
        }

        if (!unlockedItems)
        {
            unlockText.SetActive(false);
        }

    }

    private void ShowImage(Transform parent, Sprite sprite)
    {
        GameObject unlockedObject = Instantiate(unlockedImagePrefab, parent);
        Image unlockedImage = unlockedObject.GetComponent<UnlockedItem>().unlockImage;
        unlockedImage.sprite = sprite;
    }

    private void OnGameLoaded()
    {
        isInitialized = true;
    }
}
