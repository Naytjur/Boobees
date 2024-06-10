using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class ZoomSlider : MonoBehaviour
{
    [SerializeField] private Slider zoomSlider;
    [SerializeField] private TMP_Text zoomValueText;

    [SerializeField] private ZoomAndNavigation zoomAndNavigation;

    public void Start()
    {
        LoadZoomValues();
    }

    public void ZoomSliderText(float zoomSpeed)
    {
        zoomValueText.text = zoomSpeed.ToString("0.00");
    }

    public void ChangeZoomSpeed()
    {
        zoomAndNavigation.zoomSpeed = zoomSlider.value;
        LoadZoomValues();
    }

    void LoadZoomValues()
    {
        zoomSlider.value = zoomAndNavigation.zoomSpeed;
        ZoomSliderText(zoomAndNavigation.zoomSpeed);
    }
}
