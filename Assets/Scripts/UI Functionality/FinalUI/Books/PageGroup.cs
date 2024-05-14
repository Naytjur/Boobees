using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageGroup : MonoBehaviour
{
    [SerializeField]
    private List<ItemInfo> itemsToDisplay = new List<ItemInfo>();
    private ItemDisplay[] itemDisplays;

    private int displayCount;
    private int totalPages;
    private int currentPage;

    private GameObject previousButton;
    private GameObject nextButton;

    private void Awake()
    {
        itemDisplays = GetComponentsInChildren<ItemDisplay>();
        displayCount = itemDisplays.Length;
        totalPages = itemsToDisplay.Count/displayCount;
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
            if((index * displayCount) + count > itemsToDisplay.Count)
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
