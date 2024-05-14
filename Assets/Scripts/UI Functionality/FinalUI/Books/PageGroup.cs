using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageGroup : MonoBehaviour
{
    [SerializeField]
    private List<ItemInfo> itemsToDisplay = new List<ItemInfo>();
    private ItemDisplay[] itemDisplays;

    private int displayCount;
    private int totalPages;
    private int currentPage = 0;

    [SerializeField]
    private GameObject previousButton;
    [SerializeField]
    private GameObject nextButton;

    private void Awake()
    {
        itemDisplays = GetComponentsInChildren<ItemDisplay>();

        displayCount = itemDisplays.Length;
        totalPages = (int) Mathf.Ceil((float) itemsToDisplay.Count/(float) displayCount);

        if(previousButton != null)
        {
            previousButton.GetComponent<Button>().onClick.AddListener(PreviousPage);
        }
        if (nextButton != null)
        {
            nextButton.GetComponent<Button>().onClick.AddListener(NextPage);
        }
    }

    private void OnEnable()
    {
        OpenPageByIndex(currentPage);
    }

    public void OpenPageByIndex(int index)
    {
        if(index < 0 || index > totalPages - 1)
        {
            Debug.Log("Index outside of page total");
            return;
        }

        int count = 0;
        foreach(ItemDisplay display in itemDisplays)
        {
            if((index * displayCount) + count >= itemsToDisplay.Count)
            {
                display.HideDisplay();
            }
            else
            {
                display.UpdateDisplay(itemsToDisplay[(index * displayCount) + count]);
            }
            count++;
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
        OpenPageByIndex(currentPage + 1);
    }

    public void PreviousPage()
    {
        OpenPageByIndex(currentPage - 1);
    }
}
