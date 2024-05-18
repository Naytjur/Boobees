using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClosePageButton : MonoBehaviour
{
    [SerializeField]
    private UIBook book;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(book.GoToPageParent);
    }
}
