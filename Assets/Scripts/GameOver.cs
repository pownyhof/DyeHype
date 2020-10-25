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
    }

    
}
