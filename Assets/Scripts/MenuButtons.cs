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

    public void QuitGame()
    {
        Application.Quit();
    }

}
