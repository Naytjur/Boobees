using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlantButton : MonoBehaviour
{
    public PlantSO plant;

    [SerializeField]
    private CollectionNavigation nav;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { nav.ToPlantPage(plant); });
    }
}
