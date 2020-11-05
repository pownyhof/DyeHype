using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Text timeText;
    
    public void DisplayTime()
    {
        timeText.text = Timer.instance.GetTimeText().text;
    }
}
