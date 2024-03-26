using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraPosition : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Vector3 defaultRotation;

    private void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.eulerAngles;
    }

    public void SetCameraDefaultPosition()
    {
        SetCameraPosition(defaultPosition, defaultRotation);
    }

    public void SetCameraPosition(Vector3 position)
    {
        Camera.main.transform.position = position;
    }

    public void SetCameraPosition(Vector3 position, Vector3 rotation)
    {
        Camera.main.transform.position = position;
        Camera.main.transform.eulerAngles = rotation;
    }
}
