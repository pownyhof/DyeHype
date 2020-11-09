using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    //-strings for rules and jokerClue------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private string rules1_E = "You have to fill out the WHOLE grid with the colors red, blue, green and yellow. Thereby in each 2x2 Box every color has to occur exactly ONCE. Furthermore squares with the same color are not allowed to touch each other vertically or horizontally ! Example red square:";
    private string rules2_E = "Additionally clues on the outside tell you how many squares of that specific color occur in that row or column. Example:";
    private string rules3_E = "Exactly 2 blue squares in that row";
    private string rules1_G = "Das GANZE Spielfeld muss mit den Farben Rot, Blau, Grün und Gelb ausgefüllt werden. Dabei muss in jeder 2x2 großen Box jede Farbe genau EINMAL vorkommen. Außerdem dürfen sich Felder derselben Farbe nicht vertikal oder horizontal berühren. Beispiel mit einem roten Feld:";
    private string rules2_G = "Zusätzlich geben Hinweise am Spielfeldrand an, wie oft Felder einer bestimmten Farbe in einer Spalte oder Reihe vorkommen. Beispiel:";
    private string rules3_G = "Genau 2 blaue Felder in dieser Reihe";

    private string joker_E = "You have one joker per game! Select one square and the joker will fill it out for you with the correct color! Have Fun!";
    private string joker_G = "Du hast einen Joker pro Spiel! Wähle ein Feld und der Joker füllt es für dich mit der richtigen Farbe aus! Viel Spaß!";

    //-strings for rules and jokerClue------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
   
    private int selectedLanguage = 0;
    private string[] rules;
    private string[] jokerClue;

    public GameObject languagePopUp;
    public Text topText;
    public Text midText;
    public Text bottomText;

    public static Language Instance;

    void Start()
    {       
        PopulateStrings();
        // if user starts application first time, show popUp so he can chose his language
        if(PlayerPrefs.GetInt("playerHasToSetLanguage", 1) == 1)
        {
            languagePopUp.SetActive(true);
        }
        // if already chosen, set text from rules
        else
        {
            SetRulesText();
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {         
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // fill arrays with strings for better storage and acces in other classes
    public void PopulateStrings()
    {
        rules = new string[6] {rules1_E, rules2_E, rules3_E, rules1_G, rules2_G, rules3_G};
        jokerClue = new string[2] { joker_E, joker_G };
    }

    // gets called when user chose a language
    public void SetLanguage(int language)
    {
        selectedLanguage = language;
        PlayerPrefs.SetInt("language", selectedLanguage);
        PlayerPrefs.SetInt("playerHasToSetLanguage", 0);
        languagePopUp.SetActive(false);
        SetRulesText();
    }

    // method to set text correct on rulesBackground
    public void SetRulesText()
    {
        if (PlayerPrefs.GetInt("language") == 0)
        {
            topText.text = rules[0];
            midText.text = rules[1];
            bottomText.text = rules[2];
        }
        else
        {
            topText.text = rules[3];
            midText.text = rules[4];
            bottomText.text = rules[5];
        }
    }

    public string[] GetRules()
    {
        return rules;
    }

    public string[] GetJokerClue()
    {
        return jokerClue;
    }
}
