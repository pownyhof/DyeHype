using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameWon : MonoBehaviour
{

    public GameObject WinPopup;
    public Text timeText;
    

    void Start()
    {
        WinPopup.SetActive(false);
        
    }

    private void OnGameCompleted()
    {
        WinPopup.SetActive(true);
        timeText.text = Timer.instance.GetTimeText().text;
    }

    private void OnEnable()
    {
        GameEvents.OnGameCompleted += OnGameCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnGameCompleted -= OnGameCompleted;
    }
}
