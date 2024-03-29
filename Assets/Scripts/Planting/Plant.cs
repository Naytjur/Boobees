using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    private void Start()
    {
        Test();
    }

    public virtual void Test()
    {
        Debug.Log("Look at me plantin");
    }
}
