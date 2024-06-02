using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationDisplay : MonoBehaviour
{
    [SerializeField]
    private LocationService locationService;
    [SerializeField]
    private TMP_Text longText;
    [SerializeField]
    private TMP_Text latText;
    [SerializeField]
    private TMP_Text debugText;

    private void Update()
    {
        longText.text = locationService.longitude.ToString();
        latText.text = locationService.latitude.ToString();
        debugText.text = locationService.debug;
    }
}
