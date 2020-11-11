using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject ErrorPopUp;
    public GameObject OptionsPopUp;
    public InputField playerName;

    public void selectLanguageInOptions(int language)
    {
        int selectedLanguage = language;
        PlayerPrefs.SetInt("language", selectedLanguage);
    }

    public void changePlayerName()
    {
        if (PlayfabManager.Instance.UpdatePlayerName(playerName.text))
        {
            OptionsPopUp.SetActive(false);
        }
        else
        {
            // Display error to user
            ErrorPopUp.SetActive(true);
        }
    }
}
