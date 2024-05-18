using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewPageGroup : PageGroup
{
    [SerializeField]
    private List<ItemInfo> itemsToDisplay = new List<ItemInfo>();
    private ItemDisplay[] itemDisplays;
    protected int displayCount;

    protected override void Awake()
    {
        base.Awake();
        itemDisplays = GetComponentsInChildren<ItemDisplay>();

        displayCount = itemDisplays.Length;
        totalPages = (int)Mathf.Ceil((float)itemsToDisplay.Count / (float)displayCount);
    }

    public override void OpenPageByIndex(int index)
    {
        base.OpenPageByIndex(index);
        int count = 0;
        foreach (ItemDisplay display in itemDisplays)
        {
            if ((index * displayCount) + count >= itemsToDisplay.Count)
            {
                display.HideDisplay();
            }
            else
            {
                display.UpdateDisplay(itemsToDisplay[(index * displayCount) + count]);
            }
            count++;
        }
    }
}
