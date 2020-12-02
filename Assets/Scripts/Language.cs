using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    //-strings for rules, jokerClue and gameWonPopUp------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private string rules1_E = "You have to fill out the WHOLE grid with the colors red, blue, green and yellow. Thereby in each 2x2 Box every color has to occur exactly ONCE. Furthermore squares with the same color are not allowed to touch each other vertically or horizontally ! Example red square:";
    private string rules2_E = "Additionally clues on the outside tell you how many squares of that specific color occur in that row or column. Example:";
    private string rules3_E = "Exactly 2 blue squares in that row";
    private string rules4_E = "New clues! If a square is labelled with 'NB' (No blue), 'NG' (No green), 'NY' (No yellow) or 'NR' (No red), then adjacent squares with that color are not (not even diagonally!) allowed to touch the square marked with 'NB','NY','GR' or 'NR'. Example with 'NB':";
    private string rules5_E = "None of the squares, marked with a red X, is allowed to be blue, because the square in the middle is marked with 'NB' (No blue)";
    private string rules6_E = "New clues! Squares labelled with a number+color, for example '1R' (1 red) or '2G' (2 green) can occur now. For example '3B' means that exactly 3 of 4 diagonally adjacent squares have to be blue. One more example:";
    private string rules7_E = "Exactly 3 of the 4 diagonally adjacent squares have to be yellow, because the square in the middle is marked with '3Y' (3 yellow)";
    private string rules8_E = "According to the same principle there are also squares marked with 'EC' (Each color). So the 4 diagonally adjacent square have to be each color (yellow, green, red and blue) exactly once.";

    private string rules1_G = "Das GANZE Spielfeld muss mit den Farben Rot, Blau, Grün und Gelb ausgefüllt werden. Dabei muss in jeder 2x2 großen Box jede Farbe genau EINMAL vorkommen. Außerdem dürfen sich Felder derselben Farbe nicht vertikal oder horizontal berühren. Beispiel mit einem roten Feld:";
    private string rules2_G = "Zusätzlich geben Hinweise am Spielfeldrand an, wie oft Felder einer bestimmten Farbe in einer Spalte oder Reihe vorkommen. Beispiel:";
    private string rules3_G = "Genau 2 blaue Felder in dieser Reihe";
    private string rules4_G = "Neue Hinweise! Wenn Felder mit 'NB' (No blue - kein blau), 'NG' (No green - kein grün), 'NY' (No yellow - kein gelb) oder 'NR' (No red - kein rot) gekennzeichnet sind, dürfen anliegende Felder nicht (Auch NICHT diagonal!) von Feldern mit genannter Farbe berührt werden. Beispiel 'NB':";
    private string rules5_G = "Kein Feld, dass mit einem roten X markiert ist, darf die Farbe blau annehmen, da dass Feld in der Mitte mit 'NB', also 'No Blue' gekennzeichnet ist.";
    private string rules6_G = "Neue Hinweise! Es gibt Felder die mit Zahl+Farbe, als Beispiel '1R' (1 red - 1 rot) oder '2G' (2 green - 2 grün) usw. gekennzeichnet sind. Als Beispiel: '3B' bedeutet dass genau 3 von den 4 diagonal anliegenden Feldern blau sein müssen. Ein weiteres Beispiel:";
    private string rules7_G = "Von den 4 diagonal anliegenden Feldern müssen GENAU 3 gelb sein, weil dass Feld in der mitte mit '3Y' (3 yellow - 3 gelb) markiert ist.";
    private string rules8_G = "Nach demselben Prinzip kann ein Feld mit 'EC' (Each color - jede Farbe) markiert sein. Somit müssen die 4 diagonal anliegenden Felder jede Farbe (gelb, grün, rot und blau) genau einmal annehmen.";

    private string rules1_S = "Todo el campo de juego debe llenarse con los colores rojo, azul, verde y amarillo.Cada color debe aparecer exactamente una vez en cada cuadro de 2x2.Además, los campos del mismo color no deben tocarse ni vertical ni horizontalmente.Ejemplo con un campo rojo:";
    private string rules2_S = "Además, las notas en el borde del campo indican la frecuencia con la que aparecen campos de cierto color en una columna o fila. Ejemplo:";
    private string rules3_S = "Exactamente 2 campos azules en esta fila";
    private string rules4_S = "¡Nuevas pistas! Si los campos están marcados con 'NB' (No blue - sin azul), 'NG' (No green - sin verde), 'NY' (No yellow, sin amarillo) o 'NR' (No red, sin rojo) están permitidos los campos adyacentes(¡ni siquiera en diagonal!) son tocados por campos con el color indicado. Ejemplo 'NB':";
    private string rules5_S = "Ningún campo marcado con una X roja puede ser azul, ya que el campo en el medio está marcado con 'NB', es decir, 'No blue'.";
    private string rules6_S = "¡Nuevas pistas! Hay campos marcados con números y colores, por ejemplo '1R' (1 red - 1 rojo) o '2G' (2 green - 2 verdes) y así sucesivamente.Por ejemplo: '3B' significa que exactamente 3 de los 4 campos adyacentes en diagonal deben ser azules. Otro ejemplo:";
    private string rules7_S = "Exactamente 3 de los 4 campos adyacentes en diagonal deben ser amarillos, porque el campo en el medio está marcado con '3Y' (3 yellow - 3 amarillos).";
    private string rules8_S = "Utilizando el mismo principio, un campo se puede marcar con 'EC' (each color - cada color). Por lo tanto, los 4 campos adyacentes en diagonal deben tomar cada color(amarillo, verde, rojo y azul)exactamente una vez";

    private string joker_E = "You have one joker per game! Select one square and the joker will fill it out for you with the correct color! Have Fun!";
    private string joker_G = "Du hast einen Joker pro Spiel! Wähle ein Feld und der Joker füllt es für dich mit der richtigen Farbe aus! Viel Spaß!";
    private string joker_S = "¡Tienes un comodín por juego! ¡Elija un campo y el comodín lo completará con el color adecuado para usted! ¡Que te diviertas!";

    private string gameWon1_E = "Your time:";
    private string gameWon2_E = "Level completed:";
    private string gameWon3_E = "Lives remaining:";
    private string gameWon4_E = "Joker used:";
    private string gameWon5_E = "Joker left:";
    private string gameWon6_E = "Total Score:";

    private string gameWon1_G = "Deine Zeit:";
    private string gameWon2_G = "Level geschafft:";
    private string gameWon3_G = "Leben übrig:";
    private string gameWon4_G = "Joker benutzt:";
    private string gameWon5_G = "Joker übrig:";
    private string gameWon6_G = "Gesamtpunktzahl:";

    private string gameWon1_S = "Tu tiempo:";
    private string gameWon2_S = "Nivel completado:";
    private string gameWon3_S = "Vidas restantes:";
    private string gameWon4_S = "Joker usado:";
    private string gameWon5_S = "Joker no usado:";
    private string gameWon6_S = "Puntaje total:";


    //-strings for rules, jokerClue and gameWonPopUp------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    public InputField playerName;
    private int selectedLanguage = 0;
    private string[] rules_english;
    private string[] rules_german;
    private string[] rules_spanish;
    private string[] jokerClue;
    private string[] gameWon_english;
    private string[] gameWon_german;
    private string[] gameWon_spanish;

    public GameObject languagePopUp;
    public GameObject ErrorPopUp;

    public static Language Instance;

    void Start()
    {       
        PopulateStrings();
        // if user starts application first time, show popUp so he can chose his language
        if(PlayerPrefs.GetInt("playerHasToSetLanguage", 1) == 1)
        {
            languagePopUp.SetActive(true);
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
        rules_english = new string[8] {rules1_E, rules2_E, rules3_E, rules4_E, rules5_E, rules6_E, rules7_E, rules8_E };
        rules_german = new string[8] {rules1_G, rules2_G, rules3_G, rules4_G, rules5_G, rules6_G, rules7_G, rules8_G };
        rules_spanish = new string[8] { rules1_S, rules2_S, rules3_S, rules4_S, rules5_S, rules6_S, rules7_S ,rules8_S };
        jokerClue = new string[3] { joker_E, joker_G, joker_S };
        gameWon_english = new string[6] { gameWon1_E,gameWon2_E,gameWon3_E,gameWon4_E,gameWon5_E,gameWon6_E};
        gameWon_german = new string[6] { gameWon1_G,gameWon2_G,gameWon3_G,gameWon4_G,gameWon5_G,gameWon6_G};
        gameWon_spanish = new string[6] { gameWon1_S,gameWon2_S,gameWon3_S,gameWon4_S,gameWon5_S,gameWon6_S};
    }

    // gets called when user chose a language
    public void SetLanguage(int language)
    {
        // updates playerName in PlayfabManager class, if it worked it returns true
        if (PlayfabManager.Instance.UpdatePlayerName(playerName.text))
        {
            // then set language player chose and deactivate starting languagePopUp
            selectedLanguage = language;
            PlayerPrefs.SetInt("language", selectedLanguage);
            PlayerPrefs.SetInt("playerHasToSetLanguage", 0);
            languagePopUp.SetActive(false);
        }
        else
        {
            // Display error to user
            ErrorPopUp.SetActive(true);
        }
    }

    // ------ Getter methods
    public string[] GetRules(int rules)
    {
        if (rules == 0)
        {
            return rules_english;
        }

        if (rules == 1)
        {
            return rules_german;
        }

        if (rules == 2)
        {
            return rules_spanish;
        }
        // if all ifs wrong, return english and hope user speaks english ^_^
        return rules_english;
    }

    public string[] GetJokerClue()
    {
        return jokerClue;
    }

    public string[] GetGameWonText(int rules)
    {
        if (rules == 0)
        {
            return gameWon_english;
        }

        if (rules == 1)
        {
            return gameWon_german;
        }

        if (rules == 2)
        {
            return gameWon_spanish;
        }
        // if all ifs wrong, return english and hope user speaks english ^_^
        return gameWon_english;
    }
}
