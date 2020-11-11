using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{

    public static PlayfabManager Instance;
    // Start is called before the first frame update
    void Start()
    {
        Login();      
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
        GetLeaderboard();
    }

    void OnError(PlayFabError error)
    {
    Debug.Log("Error while login/account creating");
    Debug.Log(error.GenerateErrorReport());
    }

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
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }

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
        foreach(var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.DisplayName + " " + item.StatValue);
        }
    }

    public bool UpdatePlayerName(string name)
    {
        if((name.Length > 2) && name.Length < 26)
        {
            var request = new UpdateUserTitleDisplayNameRequest();
            request.DisplayName = name;
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnPlayerNameResult, OnPlayFabError);
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
}
