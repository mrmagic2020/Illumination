using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LangButtonIterator : MonoBehaviour
{
    private Button button;
    private List<Locale> locales;
    private int localeIndex;
    private PlayerPrefManager playerPrefManager;
    // Start is called before the first frame update
    void Start()
    {
        playerPrefManager = gameObject.AddComponent<PlayerPrefManager>();

        button = GetComponent<Button>();
        button.onClick.AddListener(NextLocale);

        locales = LocalizationSettings.AvailableLocales.Locales;
        LocalizationSettings.SelectedLocale = locales[PlayerPrefs.GetInt(playerPrefManager.key)];
        localeIndex = GetLocaleIndex(LocalizationSettings.SelectedLocale.LocaleName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int GetLocaleIndex(string localeName)
    {
        for (int i = 0; i < locales.Count; i++)
        {
            if (locales[i].LocaleName == localeName)
            {
                return i;
            }
        }
        return -1;
    }

    private void NextLocale()
    {
        localeIndex++;
        if (localeIndex >= locales.Count)
            localeIndex = 0;
        LocalizationSettings.SelectedLocale = locales[localeIndex];
        playerPrefManager.Set(localeIndex);
        // Debug.Log($"Changed locale to {locales[localeIndex].LocaleName}");
        // SceneManager.LoadScene("Loading");
    }
}
