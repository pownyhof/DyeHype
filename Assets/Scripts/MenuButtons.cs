using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public Sprite audioOn;
    public Sprite audioOff;
    public Button audioButton;
    public Button jokerButton;

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void MuteAudio()
    {
        int audioMuted = PlayerPrefs.GetInt("audioMuted");
        if(audioMuted == 0)
        {
           PlayerPrefs.SetInt("audioMuted", 1);
            audioButton.GetComponent<Image>().sprite = audioOff;

        }
        else
        {
           PlayerPrefs.SetInt("audioMuted", 0);
            audioButton.GetComponent<Image>().sprite = audioOn;
        }
    }

    public void LoadNextLevel()
    {
        GameSettings.Instance.SetContinuePreviousGame(false);
        int nextLevel = PlayerPrefs.GetInt("selectedLevel") + 1;
        PlayerPrefs.SetInt("selectedLevel", nextLevel);
        SceneManager.LoadScene("GameScene");
    }

    public void SetPause(bool paused)
    {
        GameSettings.Instance.SetPause(paused);
    }

    public void ContinuePreviousGame(bool continueGame)
    {
        GameSettings.Instance.SetContinuePreviousGame(continueGame);
    }

    public void ExitAfterWon()
    {
        GameSettings.Instance.SetExitAfterWon(true);
    }

    public void joker()
    {
        int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
        if (jokerUsed == 0)
        {
            jokerButton.GetComponentInChildren<Text>().text = "0";
            GameEvents.OnJokerUsedMethod();
        }
    }

    public void restartGame()
    {
        GameSettings.Instance.SetContinuePreviousGame(false);
        LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
