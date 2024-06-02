using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIBook : MonoBehaviour
{
    private List<UITabButton> buttons;
    [SerializeField]
    private List<PageInfo> pages;
    [SerializeField]
    private PageInfo homePage;

    private PageInfo selectedPage;

    private void Start()
    {
        SelectTabByReference(homePage);
    }

    public void Subscribe(PageInfo page)
    {
        if (buttons == null)
        {
            buttons = new List<UITabButton>();
        }

        pages.Add(page);
    }

    public void OnTabSelected(UITabButton button)
    {
        SelectTabByReference(button.page);
    }

    public void SelectTabByReference(PageInfo reference)
    { 
        foreach (PageInfo page in pages)
        {
            if (page == reference)
            {
                page.gameObject.SetActive(true);
            }
            else
            {
                page.gameObject.SetActive(false);
            }
        }
        selectedPage = reference;
    }

    public void GoToPageParent()
    {
        Debug.Log(selectedPage.parent);
        if(selectedPage.parent == null)
        {
            CloseCatalogue();
            return;
        }
        SelectTabByReference(selectedPage.parent);
    }

    public void CloseCatalogue()
    {
        SelectTabByReference(homePage);
        GameManager.instance.UpdateGameState(GameState.Viewing);
    }
}
