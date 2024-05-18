using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UITabButton : MonoBehaviour
{
    [SerializeField]
    private UIBook tabGroup;
    private Button button;
    public PageInfo page;

    private void Awake()
    {
        if (tabGroup == null)
        {
            tabGroup = GetComponentInParent<UIBook>();
        }
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        tabGroup.OnTabSelected(this);
    }
}
