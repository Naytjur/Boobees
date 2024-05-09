using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObject : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyable;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(DestroyDestroyable);
    }
    private void DestroyDestroyable()
    {
        Destroy(destroyable);
    }
}
