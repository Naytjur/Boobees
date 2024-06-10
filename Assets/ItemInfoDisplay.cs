using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class ItemInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private LocalizeStringEvent stringEvent;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateText(LocalizedString localString)
    {
        stringEvent.StringReference = localString;
        stringEvent.RefreshString();
    }
}
