using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageGroup : MonoBehaviour
{

    protected int totalPages;
    protected int currentPage = 0;

    [SerializeField]
    private GameObject previousButton;
    [SerializeField]
    private GameObject nextButton;

    protected virtual void Awake()
    {
        if(previousButton != null)
        {
            previousButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
        }
        if (nextButton != null)
        {
            nextButton.GetComponent<Button>().onClick.AddListener(NextPage);
        }
    }

    protected virtual void OnEnable()
    {
        OpenPageByIndex(currentPage);
    }

    public virtual void OpenPageByIndex(int index)
    {
        if(index < 0 || index > totalPages - 1)
        {
            Debug.Log("Index outside of page total");
            return;
        }

        currentPage = index;

        previousButton.SetActive(true);
        nextButton.SetActive(true);

        if(currentPage == 0)
        {
            previousButton.SetActive(false);
        }
        if(currentPage >= totalPages - 1)
        {
            nextButton.SetActive(false);
        }
    }

    public void NextPage()
    {
        Debug.Log(currentPage);
        OpenPageByIndex(currentPage + 1);
    }

    public void PreviousPage()
    {
        OpenPageByIndex(currentPage - 1);
    }
}
