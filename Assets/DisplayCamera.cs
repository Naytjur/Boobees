using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCamera : MonoBehaviour
{
    [SerializeField]
    private RawImage display = default;

    private Texture defaultBackground;
    private WebCamTexture cam;

    private void Start()
    {
        defaultBackground = display.texture;
    }

    private void OnEnable()
    {
        DisplayCameraImage();
    }

    private void OnDisable()
    {
        HideCameraImage();
    }

    private void HideCameraImage()
    {
        cam.Stop();
        display.texture = defaultBackground;
    }

    private void DisplayCameraImage()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        if(devices.Length == 0)
        {
            return;
        }

        foreach(WebCamDevice device in devices)
        {
            if(!device.isFrontFacing)
            {
                cam = new WebCamTexture(device.name, Screen.width, Screen.height);
            }
        }

        if(cam == null)
        {
            return;
        }

        cam.Play();
        display.texture = cam;
    }
}
