using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    private int selectedLevel = 0;

    void Start()
    {
        PlayerPrefs.SetInt("selectedLevel", selectedLevel);
    }

}
