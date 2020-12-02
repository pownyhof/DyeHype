using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOutputHandler : MonoBehaviour
{
    public static LogOutputHandler Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    //send logs to Loggly
    public void HandleClickLog(string eventType, string playerId, string level, string field, string enteredColor, string correct, string gameId, string clickTime)
    {
        //Initialize WWWForm
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("Event_Type", eventType);
        loggingForm.AddField("Player_Id", playerId);
        loggingForm.AddField("Level", level);
        loggingForm.AddField("Square_Index", field);
        loggingForm.AddField("Color", enteredColor);
        loggingForm.AddField("Correct", correct);
        loggingForm.AddField("Game_Id", gameId);
        loggingForm.AddField("Click_Time_In_Sec", clickTime);

        StartCoroutine(SendData(loggingForm));
    }

    public void HandleLevelCompletedLog(string eventType, string playerId, string level, string gameId, string requiredTime, string mistakes, string joker)
    {
        //Initialize WWWForm
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("Event_Type", eventType);
        loggingForm.AddField("Player_Id", playerId);
        loggingForm.AddField("Level", level);
        loggingForm.AddField("Game_Id", gameId);
        loggingForm.AddField("Required_Time_In_Sec", requiredTime);
        loggingForm.AddField("Mistakes", mistakes);
        loggingForm.AddField("Joker_Used", joker);

        StartCoroutine(SendData(loggingForm));
    }

    public void HandleGameOverLog(string eventType, string playerId, string level, string gameId, string requiredTime)
    {
        //Initialize WWWForm
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("Event_Type", eventType);
        loggingForm.AddField("Player_Id", playerId);
        loggingForm.AddField("Level", level);
        loggingForm.AddField("Game_Id", gameId);
        loggingForm.AddField("Required_Time_In_Sec", requiredTime);

        StartCoroutine(SendData(loggingForm));
    }

    public void HandleGameStartedLog(string eventType, string playerId, string level, string gameId)
    {
        //Initialize WWWForm
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("Event_Type", eventType);
        loggingForm.AddField("Player_Id", playerId);
        loggingForm.AddField("Level", level);
        loggingForm.AddField("Game_Id", gameId);

        StartCoroutine(SendData(loggingForm));
    }

    public void HandleRatingLog(string eventType, string playerId, string level, string rating)
    {
        //Initialize WWWForm
        var loggingForm = new WWWForm();

        //Add log message to WWWForm
        loggingForm.AddField("Event_Type", eventType);
        loggingForm.AddField("Player_Id", playerId);
        loggingForm.AddField("Level", level);
        loggingForm.AddField("Rating", rating);

        StartCoroutine(SendData(loggingForm));
    }

    public IEnumerator SendData(WWWForm form)
    {
        Debug.Log("made it");
        //Send WWW Form to Loggly, replace TOKEN with your unique ID from Loggly
        WWW sendLog = new WWW("http://logs-01.loggly.com/inputs/c6feb7bf-b09e-44cc-aff8-bff38f455b51/tag/Unity3D", form);
        yield return sendLog;
    }
}
