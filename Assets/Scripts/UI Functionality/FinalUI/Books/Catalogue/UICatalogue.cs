using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICatalogue : UIBook
{
    [SerializeField]
    private PageInfo plantInfoPage;
    [SerializeField]
    private PageInfo beeInfoPage;
    [SerializeField]
    private PageInfo butterflyInfoPage;

    public void OpenInfoPage(CatalogueItemInfo info)
    {
        if (GameManager.instance.state != GameState.Catalogue)
        {
            GameManager.instance.UpdateGameState(GameState.Catalogue);
        }

        PageInfo infoPage = plantInfoPage;

        if(info is PlantSO plant)
        {
            infoPage = plantInfoPage;
        }
        if (info is InsectSO insect)
        {
            if(insect.type == InsectSO.Type.Bee)
            {
                infoPage = beeInfoPage;
            }
            if (insect.type == InsectSO.Type.Butterfly)
            {
                infoPage = butterflyInfoPage;
            }
        }


        SelectTabByReference(infoPage);
        infoPage.GetComponent<InfoPageGroup>().UpdateInfo(info);
    }
}
