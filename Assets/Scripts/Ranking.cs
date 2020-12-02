using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    List<PlayfabManager.rankingEntry> result;
    string userID;

    public Text currentPlayerPos;
    public Text currentPlayerName;
    public Text currentPlayerScore;

    public GameObject errorPopUp;

    void Start()
    {
        // get id from user playing
        userID = PlayfabManager.Instance.GetUserID();
        // get whole leaderboard
        result = PlayfabManager.Instance.GetLeaderboardList();
        PopulateRankingList();
    }

    public void PopulateRankingList()
    {
        GameObject entryTemplate = transform.GetChild(0).gameObject;
        GameObject g;
        // if result was loaded correctly
        if (result.Count != 0)
        {
            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].userID == userID)
                {
                    g = Instantiate(entryTemplate, transform);
                    g.transform.GetChild(0).GetComponent<Text>().text = result[i].pos;
                    g.transform.GetChild(1).GetComponent<Text>().text = result[i].name;
                    g.transform.GetChild(2).GetComponent<Text>().text = result[i].score;
                    // if leaderboard data is from user currently playing, make his entry text bold and show his "stats" additionally on the bottom of RankingPopUp
                    g.transform.GetChild(0).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    g.transform.GetChild(1).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    g.transform.GetChild(2).GetComponent<Text>().fontStyle = FontStyle.Bold;
                    currentPlayerPos.text = result[i].pos;
                    currentPlayerName.text = result[i].name;
                    currentPlayerScore.text = result[i].score;

                }
                else
                {
                    // normal entry from other players
                    g = Instantiate(entryTemplate, transform);
                    g.transform.GetChild(0).GetComponent<Text>().text = result[i].pos;
                    g.transform.GetChild(1).GetComponent<Text>().text = result[i].name;
                    g.transform.GetChild(2).GetComponent<Text>().text = result[i].score;
                }
            }
            errorPopUp.SetActive(false);
            Destroy(entryTemplate);          
        }
        // if no result list exist, for example user had no internet connection
        else
        {
            // show error
            errorPopUp.SetActive(true);
        }
    }
}
