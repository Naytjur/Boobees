using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum ResourceType
{
    Honey,
    Pollen
}

public class ResourceSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private ResourceType resource;

    private void Update()
    {
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        float max = 0;
        float value = 0;

        switch (resource)
        {
            case ResourceType.Honey:
                max = ScoreManager.instance.maxHoneyScore;
                value = ScoreManager.instance.honeyScore;
                
                break;
            case ResourceType.Pollen:
                max = ScoreManager.instance.maxPollenScore;
                value = ScoreManager.instance.pollenScore;
                break;
        }

        text.text = value.ToString() + "/" + max.ToString();
        slider.maxValue = max;
        slider.value = value;
    }
}
