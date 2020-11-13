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
            PlayerPrefs.SetInt("levelReached", (levelReached));
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
        int total = 100;
        int livesRemainingScore = 0;
        int jokerScore = 0;

        livesRemaining = Lives.GetRemainingLives();
        int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
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
        showScore(100,livesRemainingScore,jokerScore,total);
        return total;
    }

    private void showScore(int levelCompletedScore, int livesRemainingScore, int jokerScore, int total)
    {
        levelCompletedScoreText.text = levelCompletedScore.ToString();
        livesRemainingScoreText.text = livesRemainingScore.ToString();
        jokerUsedScoreText.text = jokerScore.ToString();
        totalScoreText.text = total.ToString();
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
