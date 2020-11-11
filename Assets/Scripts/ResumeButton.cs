using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ResumeButton : MonoBehaviour
{
    public Text timeText;
    public Text levelText;

    string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }

    void Start()
    {
        // deactivate resume button if no gameData.ini exists
        if(Config.GameFileExist() == false)
        {
            gameObject.GetComponent<Button>().interactable = false;
            timeText.text = " ";
            levelText.text = " ";
        }
        // else set text and level to resumeButton
        else
        {
            float delta_time = Config.ReadTime();
            delta_time += Time.deltaTime;
            TimeSpan span = TimeSpan.FromSeconds(delta_time);

            string hour = LeadingZero(span.Hours);
            string min = LeadingZero(span.Minutes);
            string sec = LeadingZero(span.Seconds);

            timeText.text = hour + ":" + min + ":" + sec;

            int currentLevel = PlayerPrefs.GetInt("selectedLevel") + 1;
            levelText.text = "Level " + currentLevel.ToString();
        }
    }
}
