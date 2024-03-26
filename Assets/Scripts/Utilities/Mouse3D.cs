using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse3D
{
    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }

        return new Vector3(-1000, -1000, -1000);
    }

    public static Vector3 GetMouseWorldPosition(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layer))
        {
            return hit.point;
        }

        return new Vector3(-1000, -1000, -1000);
    }
}
