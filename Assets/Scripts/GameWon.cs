﻿using System.Collections;
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
        int levelReached = PlayerPrefs.GetInt("levelReached") + 1;
        PlayerPrefs.SetInt("levelReached", levelReached);
        timeText.text = Timer.instance.GetTimeText().text;
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