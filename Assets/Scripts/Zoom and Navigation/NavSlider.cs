using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class NavSlider : MonoBehaviour
{
    [SerializeField] private Slider navSlider;
    [SerializeField] private TMP_Text navValueText;

    [SerializeField] private ZoomAndNavigation zoomAndNavigation;


    public void Start()
    {
        LoadNavValues();
    }
    public void NavSliderText(float navSpeed)
    {
        navValueText.text = navSpeed.ToString("0.00");
    }
    public void ChangeNavSpeed()
    {
        zoomAndNavigation.navSpeed = navSlider.value;
        LoadNavValues();
    }

    void LoadNavValues()
    {
        navSlider.value = zoomAndNavigation.navSpeed;
        NavSliderText(zoomAndNavigation.navSpeed);
    }
}
