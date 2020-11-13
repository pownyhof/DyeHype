using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;
    public string[] stringResult;
    public string currentUserPlayfabID;
    private static bool isLogin = false;

    // list used to save leaderboard info, list is used later in ranking.cs
    public List<PlayfabManager.rankingEntry> rankingList = new List<PlayfabManager.rankingEntry>();

    public struct rankingEntry
    {
        public string pos;
        public string name;
        public string score;
        public string userID;

        public rankingEntry(string userPlayfabID, string posData, string nameData, string scoreData) : this()
        {
            this.userID = userPlayfabID;
            this.pos = posData;
            this.name = nameData;
            this.score = scoreData;
        }
    };

    void Start()
    {
        // if player did not yet log in
        if (!isLogin)
        {
            Login();
        }
        else
        {
            // if player is already logged in because hes navigating via backbuttons from gameScene to Menu, immediately get leaderboard info so user can see ranking without delay
            GetLeaderboard();
            GetAccountInfo();
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

    void Login()
    {
        var request = new LoginWithCustomIDRequest { 
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    
    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account created!");
        isLogin = true;
        // immediately after successfull login, get leaderboard and player info and save it to static variables, so it can be later immediately used in ranking class without delay for the user
        GetLeaderboard();
        GetAccountInfo();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while login/account creating");
        Debug.Log(error.GenerateErrorReport());
    }

    // method gets called onGameWon event, when player completed a level successfully
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "ranking",
                    Value = score
                }
            }
        };
        // value gets added up automatically to players score and leaderboard gets sorted automatically too
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

    // get list and ID from player whos currently playing, both variables used later in Ranking.cs
    public List<PlayfabManager.rankingEntry> GetLeaderboardList()
    {
        return rankingList;
    }

    public string GetUserID()
    {
        return currentUserPlayfabID;
    }

    // method to get all entries in leaderboard
    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "ranking",
            StartPosition = 0,
            MaxResultsCount = 100
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        // save data from Playfab Database in a list that is needed to show leadboard in rankingPopUp later on
        foreach(var item in result.Leaderboard)
        {
            int pos = item.Position + 1;
            rankingList.Add(new PlayfabManager.rankingEntry(item.PlayFabId, pos.ToString(), item.DisplayName, item.StatValue.ToString()));
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }

    // gets called when player first login and chooses name or when he wants to change name in options
    public bool UpdatePlayerName(string name)
    {
        // name has to be between 3 and 25 characters
        if((name.Length > 2) && name.Length < 26)
        {
            var request = new UpdateUserTitleDisplayNameRequest();
            request.DisplayName = name;
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnPlayerNameResult, OnPlayFabError);
            // return if request and changing name was successfull
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        Debug.Log("Error: " + obj.Error);
    }

    private void OnPlayerNameResult(UpdateUserTitleDisplayNameResult obj)
    {
        Debug.Log("playername: " + obj.DisplayName);
    }

    // method to get playfabID from user whos currently playing
    void GetAccountInfo()
    {
        GetAccountInfoRequest request = new GetAccountInfoRequest();
        PlayFabClientAPI.GetAccountInfo(request, Successs, fail);
    }

    // onSuccess save info it in static variable to use later on in ranking class
    void Successs(GetAccountInfoResult result)
    {
        currentUserPlayfabID = result.AccountInfo.PlayFabId;
    }

    void fail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}
