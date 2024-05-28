using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelPopUp : MonoBehaviour
{
    [SerializeField]
    private GameObject popUpPrefab;
    [SerializeField]
    private GameObject popUpCanvas;
    [SerializeField]
    private GameObject unlockedImagePrefab;

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

        foreach (BuildingSO building in BuildManager.instance.GetBuildingList())
        {
            if (building.unlockLevel == level)
            {
                ShowImage(popUpInfo.unlockedImagesTransform, building.sprite);
            }
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
