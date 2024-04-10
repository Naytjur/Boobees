using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mouse3D
{
    public static bool GetMouseWorldPosition(out Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }

    public static bool GetMouseWorldPosition(LayerMask layer, out Vector3 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, layer))
        {
            pos = hit.point;
            return true;
        }
        pos = Vector3.zero;
        return false;
    }
}
