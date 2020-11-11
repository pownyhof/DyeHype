using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    private bool continuePreviousGame = false;
    private bool exitAfterWon = false;
    private bool paused = false;


    public static GameSettings Instance;

    public bool GetPause()
    {
        return paused;
    }

    public void SetPause(bool set)
    {
        paused = set;
    }

    // gets set when player completes a level, so gameData wont be saved for resume button 
    public void SetExitAfterWon(bool set)
    {
        exitAfterWon = set;
        continuePreviousGame = false;
    }

    public bool GetExitAfterWon()
    {
        return exitAfterWon;
    }

    // if true gameData.ini gets loaded, else new game
    public void SetContinuePreviousGame(bool set)
    {
        continuePreviousGame = set;
    }

    public bool GetContinuePreviousGame()
    {
        return continuePreviousGame;
    }

    private void Awake()
    {
        paused = false;
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        continuePreviousGame = false;
    }
    
}
