using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class ScanSeedsPopUpInfo : MonoBehaviour
{
    public Image flowerImage;
    public LocalizeStringEvent nameLocalizeStringEvent;
    public int addedSeedAmount = 0;
    public LocalizeStringEvent seedsLocalizeStringEvent;
}
