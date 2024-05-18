using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITabButton : MonoBehaviour
{
    [SerializeField]
    private TabGroup tabGroup;

    private Button button;

    private void Awake()
    {
        if (tabGroup == null)
        {
            tabGroup = GetComponentInParent<TabGroup>();
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        tabGroup.Subscribe(this); 
    }

    private void OnClick()
    {
        tabGroup.OnTabSelected(this);
    }
}
