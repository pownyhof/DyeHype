using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{

    public GameObject WinPopup;
    public Text timeText;
    public AudioSource sound;

    public List<GameObject> live_images;

    private int livesRemaining;


    void Start()
    {
        WinPopup.SetActive(false);
        
    }

    private void OnGameCompleted()
    {
        int audioMuted = PlayerPrefs.GetInt("audioMuted");
        if (audioMuted == 0)
        {
            sound.Play();
        }

        // player unlocked new level
        int levelReached = PlayerPrefs.GetInt("levelReached") + 1;
        PlayerPrefs.SetInt("levelReached", levelReached);

        // get time player needed from Timer.cs
        timeText.text = Timer.instance.GetTimeText().text;

        // get lives left from Lives.cs
        livesRemaining = Lives.GetRemainingLives();

        // show how many lives player had left
        for(int i = 0; i < livesRemaining; i++)
        {
            live_images[i].SetActive(true);
        }



        WinPopup.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvents.OnGameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameCompleted -= OnGameCompleted;
    }
}
