using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomAndNavigation : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID

    public Camera mainCamera;
    private Plane map;
    [SerializeField, Range(0f, 10f)]
    private float navSpeed = 1;
    [SerializeField, Range(0f, 2f)]
    private float zoomSpeed = 1;


    [SerializeField]
    private Collider boundingBox;


    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        {
            return;
        }

        if (GameManager.instance.state == GameState.Viewing)
        {
            if (Input.touchCount >= 1)
            {
                map.SetNormalAndPosition(transform.up, transform.position);
            }

            var delta1 = Vector3.zero;

            //navigation
            if (Input.touchCount == 1)
            {
                delta1 = MapPositionDelta(Input.GetTouch(0));
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    mainCamera.transform.Translate(delta1 / navSpeed, Space.World);
                

                }
            }

            // Zooming
            if (Input.touchCount >= 2)
            {
                Vector3 pos1Old = MapPosition(Input.GetTouch(0).position);
                Vector3 pos2Old = MapPosition(Input.GetTouch(1).position);
                Vector3 pos1New = MapPosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                Vector3 pos2New = MapPosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                float zoom = (Vector3.Distance(pos1Old, pos2Old) /
                             Vector3.Distance(pos1New, pos2New));

                zoom -= 1;
                zoom *= zoomSpeed;

                if (zoom == 0)
                {
                    return;
                }

                zoom += 1;

                Mathf.Clamp(zoom, 0, 2);

                mainCamera.transform.position = Vector3.LerpUnclamped(pos1Old, mainCamera.transform.position, 1/ zoom);
              
            }
            ContrainCamera();
        }

    }

    private Vector3 MapPositionDelta(Touch touch)
    {
        // not moved
        if (touch.phase != TouchPhase.Moved)
        {
            return Vector3.zero;
        }

        // delta
        Ray rayBefore = mainCamera.ScreenPointToRay(touch.position - touch.deltaPosition);
        Ray rayNow = mainCamera.ScreenPointToRay(touch.position);
        if (map.Raycast(rayBefore, out float enterBefore) && map.Raycast(rayNow, out float enterNow))
        {
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);
        }

        // not on map
        return Vector3.zero;
    }

    private Vector3 MapPosition(Vector2 screenPos)
    {
        Ray rayNow = mainCamera.ScreenPointToRay(screenPos);
        if (map.Raycast(rayNow, out float enterNow))
        {
            return rayNow.GetPoint(enterNow);
        }

        return Vector3.zero;
    }

    private void ContrainCamera()
    {
        mainCamera.transform.localPosition = new Vector3(
                Mathf.Clamp(mainCamera.transform.localPosition.x, boundingBox.bounds.min.x, boundingBox.bounds.max.x),
                Mathf.Clamp(mainCamera.transform.localPosition.y, boundingBox.bounds.min.y, boundingBox.bounds.max.y),
                Mathf.Clamp(mainCamera.transform.localPosition.z, boundingBox.bounds.min.z, boundingBox.bounds.max.z)
                );

    }
    

#endif
}
