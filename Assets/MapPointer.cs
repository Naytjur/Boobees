using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPointer : MonoBehaviour
{
    [SerializeField]
    private LocationService locationService;
    [SerializeField]
    private Transform pointer;
    [SerializeField]
    private Image map;
    [SerializeField]
    private Vector2 worldOrigin;
    [SerializeField]
    private float areaWidth;
    [SerializeField]
    private float areaHeight;

    private float mapWidth;
    private float mapHeight;

    private float widthScale;
    private float heightScale;

    private void Start()
    {
        mapWidth = 2400;
        mapHeight = 1080;

        widthScale = mapWidth / areaWidth;
        heightScale = mapHeight / areaHeight;
    }

    private void Update()
    {
        if(locationService.longitude == 0)
        {
            pointer.gameObject.SetActive(false);
            return;
        }
        UpdatePointerPosition();
    }

    private void UpdatePointerPosition()
    {
        Vector2 position = Vector3.Lerp(pointer.transform.position, ConvertCoordsToXY(new Vector2(locationService.longitude, locationService.latitude)), Time.deltaTime * 20f);

        if (IsPositionOnMap(position))
        {
            pointer.position = position;
            pointer.gameObject.SetActive(true);
        }
        else
        {
            pointer.gameObject.SetActive(false);
        }
    }

    private Vector2 ConvertCoordsToXY(Vector2 coords)
    {
        float x = coords.x;
        float y = coords.y;

        x -= worldOrigin.x;
        y -= worldOrigin.y;

        x *= widthScale;
        y *= heightScale;

        return new Vector2(x, y);
    }

    private bool IsPositionOnMap(Vector2 position)
    {
        if(position.x <= mapWidth && position.x >= 0 && position.y <= mapHeight && position.y >= 0)
        {
            return true;
        }
        return false;
    }
}
