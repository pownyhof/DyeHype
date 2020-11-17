using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{

    public GameObject WinPopup;
    public Text textOverTime;
    public Text timeText;
    public AudioSource sound;

    public List<GameObject> live_images;

    private int livesRemaining;
    private string[] gameWonText;

    public Text levelCompleted;
    public Text levelCompletedScoreText;
    public Text livesRemainingText;
    public Text livesRemainingScoreText;
    public Text jokerUsed;
    public Text jokerUsedScoreText;
    public Text total;
    public Text totalScoreText;


    void Start()
    {
        SetLanguage();
        WinPopup.SetActive(false);   
    }

    private void OnGameCompleted()
    {
        // if player didnt mute audio play winSound
        int audioMuted = PlayerPrefs.GetInt("audioMuted");
        if (audioMuted == 0)
        {
            sound.Play();
        }

        // player unlocked new level
        int currentLevel = PlayerPrefs.GetInt("selectedLevel");
        int levelReached = PlayerPrefs.GetInt("levelReached");
        if (currentLevel == levelReached)
        {
            levelReached += 1;
            PlayerPrefs.SetInt("levelReached", levelReached);
            int score = calculateScore();
            PlayfabManager.Instance.SendLeaderboard(score);
        }

        // get time player needed from Timer.cs
        timeText.text = Timer.instance.GetTimeText().text;

        // get lives left from Lives.cs
        livesRemaining = Lives.GetRemainingLives();

        // show how many lives player had left
        for(int i = 0; i < livesRemaining; i++)
        {
            live_images[i].SetActive(true);
        }
        // show GameObject winPopUp
        WinPopup.SetActive(true);
    }

    private int calculateScore()
    {
        int levelScore = 0;
        int total = 0;
        int livesRemainingScore = 0;
        int jokerScore = 0;

        livesRemaining = Lives.GetRemainingLives();
        int levelReached = PlayerPrefs.GetInt("levelReached");
        int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
        if(levelReached > 0)
        {
            levelScore += 100;
            if(levelReached > 9)
            {
                levelScore += 50;
                if(levelReached > 19)
                {
                    levelScore += 50;
                    if (levelReached > 29)
                    {
                        levelScore += 50;
                        if (levelReached > 39)
                        {
                            levelScore += 50;                         
                        }
                    }
                }
            }
        }
        if(livesRemaining == 3)
        {
            livesRemainingScore = 60;
            total += 60;
        }
        if(livesRemaining == 2)
        {
            livesRemainingScore = 40;
            total += 40;
        }
        if (livesRemaining == 1)
        {
            livesRemainingScore = 20;
            total += 20;
        }
        if (jokerUsed == 0)
        {
            jokerScore = 40;
            total += 40;
        }
        total += levelScore;
        showScore(levelScore,livesRemainingScore,jokerScore,total);
        return total;
    }

    private void showScore(int levelCompletedScore, int livesRemainingScore, int jokerScore, int total)
    {
        levelCompletedScoreText.text = "+" + levelCompletedScore.ToString() + " p";
        livesRemainingScoreText.text = "+" + livesRemainingScore.ToString() + " p";
        jokerUsedScoreText.text = "+" + jokerScore.ToString() + " p";
        totalScoreText.text = "+" + total.ToString() + " p";
    }

    private void SetLanguage()
    {
        // check which language player has selected
        if (PlayerPrefs.GetInt("language") == 0)
        {
            gameWonText = Language.Instance.GetGameWonText(0);

        }
        if (PlayerPrefs.GetInt("language") == 1)
        {
            gameWonText = Language.Instance.GetGameWonText(1);

        }
        if (PlayerPrefs.GetInt("language") == 2)
        {
            gameWonText = Language.Instance.GetGameWonText(2);
        }

        textOverTime.text = gameWonText[0];
        levelCompleted.text = gameWonText[1];
        livesRemainingText.text = gameWonText[2];
        int jokerUsedbool = PlayerPrefs.GetInt("jokerUsed");
        if (jokerUsedbool == 0)
        {
            jokerUsed.text = gameWonText[4];
        }
        else
        {
            jokerUsed.text = gameWonText[3];
        }
        total.text = gameWonText[5];
    }

    private void OnEnable()
    {
        // subscribe to event
        GameEvents.OnGameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        // unsubscribe to event
        GameEvents.OnGameCompleted -= OnGameCompleted;
    }
}
