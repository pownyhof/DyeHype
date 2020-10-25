using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{

    private Text timerText;
    private float delta_time;
    private bool stopTimer = false;

    public static Timer instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }

        instance = this;

        timerText = GetComponent<Text>();
        delta_time = 0;
    }

    void Start()
    {
        stopTimer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(stopTimer == false)
        {
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string hour = LeadingZero(span.Hours);
            string min = LeadingZero(span.Minutes);
            string sec = LeadingZero(span.Seconds);

            timerText.text = hour + ":" + min + ":" + sec;
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    public void OnGameOver()
    {
        stopTimer = true;
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= OnGameOver;
    }

    public Text GetTimeText()
    {
        return timerText;
    }
}
