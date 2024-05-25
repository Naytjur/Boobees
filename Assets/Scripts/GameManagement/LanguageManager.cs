using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{
    private bool active = false;

    public void ChangeLanguage(int languageID)
    {
        if (active)
        {
            return;
        }
        StartCoroutine(SetLanguage(languageID));
    }

    IEnumerator SetLanguage(int _languageID)
    {
        active= true;
        yield return LocalizationSettings.InitializationOperation; //Checks if localization system is ready to be used 
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_languageID];
        active = false;
    }
}
