using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPageGroup : PageGroup
{
    [SerializeField]
    private GameObject[] pages;

    [SerializeField]
    private Image image;
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private ItemDisplay[] itemReferences;

    protected override void Awake()
    {
        base.Awake();
        totalPages = pages.Length;
    }

    public void UpdateInfo(CatalogueItemInfo info)
    {
        image.sprite = info.sprite;
        title.text = info.itemName;
        description.text = info.description;

        if(info.attractions.Length > 0)
        {
            int count = 0;
            foreach (ItemDisplay display in itemReferences)
            {
                if (count >= info.attractions.Length)
                {
                    display.HideDisplay();
                }
                else
                {
                    display.UpdateDisplay(info.attractions[count]);
                }
                count++;
            }
        }
    }

    public override void OpenPageByIndex(int index)
    {
        base.OpenPageByIndex(index);
        for(int i = 0; i < pages.Length; i++)
        {
            if(index == i)
            {
                pages[i].gameObject.SetActive(true);
            }
            else
            {
                pages[i].gameObject.SetActive(false);
            }
        }
    }
}
