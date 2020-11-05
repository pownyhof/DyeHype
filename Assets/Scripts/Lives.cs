using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    // list with red X pictures
    public List<GameObject> error_images;
    public GameObject gameOverPopUp;

    private static int lives = 0;
    int errorCount = 0;

    public static Lives instance;

    void Start()
    {
        
        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            errorCount = Config.ErrorNumber();
            lives = error_images.Count - errorCount;

            for(int error = 0; error < errorCount; error++)
            {
                error_images[error].SetActive(true);
            }
        }
        else
        {
            lives = error_images.Count;
            errorCount = 0;
        }
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void checkGameOver()
    {
        if(lives <= 0)
        {
            GameEvents.OnGameOverMethod();
            gameOverPopUp.SetActive(true);
        }
    }

    private void WrongColor()
    {
        if(errorCount < error_images.Count)
        {
            error_images[errorCount].SetActive(true);
            errorCount++;
            lives--;
        }
        checkGameOver();
    }

    private void OnEnable()
    {
        GameEvents.OnWrongColor += WrongColor;
    }

    private void OnDisable()
    {
        GameEvents.OnWrongColor -= WrongColor;
    }

    public static int GetRemainingLives()
    {
        return lives;
    }

    public int GetErrorNumber()
    {
        return errorCount;
    }

}

