using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{ 
    public Text timerText;
    void Start()
    {
        timerText.text = Timer.instance.GetTimeText().text;

        // logging
        int level = PlayerPrefs.GetInt("selectedLevel") + 1;
        int levelTry = PlayerPrefs.GetInt("levelTry");
        string playerId = PlayfabManager.Instance.GetUserID();
        string gameId = playerId + "_" + level.ToString() + "_" + levelTry.ToString();
        LogOutputHandler.Instance.HandleGameOverLog("GAME_OVER", playerId, level.ToString(), gameId, Timer.instance.GetCurrentTimeForLogging());

        levelTry += 1;
        PlayerPrefs.SetInt("levelTry", levelTry);
    }  
}
