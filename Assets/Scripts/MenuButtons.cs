using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Sprite audioOn;
    public Sprite audioOff;
    public Button audioButton;

    public GameObject optionsPopUp;
    public GameObject rankingPopUp;

    void Update()
    {
        // If player uses Android backarrow instead of menu back buttons
        if ((Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Menu") && (optionsPopUp.activeSelf == false) && (rankingPopUp.activeSelf == false))
        {
            QuitGame();
        }

        if ((Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Menu") && (optionsPopUp.activeSelf == true))
        {
            optionsPopUp.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "Menu") && (rankingPopUp.activeSelf == true))
        {
            rankingPopUp.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "GameScene")
        {
            SceneManager.LoadScene("LevelMenu");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "LevelMenu")
        {
            SceneManager.LoadScene("Menu");
        }
    }

    // universal method to load new scenes
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // method if player wants to mute audio
    public void MuteAudio()
    {
        // gets saved in player prefs
        int audioMuted = PlayerPrefs.GetInt("audioMuted");
        if (audioMuted == 0)
        {
            // change player prefs settings and icon in GameScene
            PlayerPrefs.SetInt("audioMuted", 1);
            audioButton.GetComponent<Image>().sprite = audioOff;
        }
        else
        {
            PlayerPrefs.SetInt("audioMuted", 0);
            audioButton.GetComponent<Image>().sprite = audioOn;
        }
    }

    // method for "more!" Button, if player wants to continue playing directly after gameWonPopUp
    public void LoadNextLevel()
    {
        GameSettings.Instance.SetContinuePreviousGame(false);
        // current level gets always saved in player prefs
        int nextLevel = PlayerPrefs.GetInt("selectedLevel") + 1;
        // so user can't load level that does not exist, currently 60 levels in game
        int maxLevel = 60;
        if (nextLevel < maxLevel)
        {
            PlayerPrefs.SetInt("selectedLevel", nextLevel);
            SceneManager.LoadScene("GameScene");
        }
    }

    public void SetPause(bool paused)
    {
        // if player presse pause icon
        GameSettings.Instance.SetPause(paused);
    }

    public void ContinuePreviousGame(bool continueGame)
    {
        // if true load saved .ini file, else load level that player selected
        GameSettings.Instance.SetContinuePreviousGame(continueGame);
    }


    public void ExitAfterWon()
    {
        // set this to true when player completed a level, so gameData to continue a game wont be saved in .ini file
        GameSettings.Instance.SetExitAfterWon(true);
    }

    // method when player presses joker(crown) icon
    public void joker()
    {
        int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
        // just use Joker if player didnt use one already in current level
        if (jokerUsed == 0)
        {
            GameEvents.OnJokerUsedMethod();
        }
    }

    // adMob
    public void playAd()
    {
        AdManager.Instance.ShowInterstitialAd();
    }

    // logging
    public void logRating(int rating)
    {
        int level = PlayerPrefs.GetInt("selectedLevel") + 1;
        string playerId = PlayfabManager.Instance.GetUserID();
        LogOutputHandler.Instance.HandleRatingLog("RATING", playerId, level.ToString(), rating.ToString());
    }

    // method when player presses "new try" button in GameScene
    public void restartGame()
    {
        GameSettings.Instance.SetContinuePreviousGame(false);
        LoadScene("GameScene");
    }

    // Quit Application
    public void QuitGame()
    {
        Application.Quit();
    }

}