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
        // when player resumes a game set how many live player has remaining
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

    // if player made 3 mistakes call GameOver event and show gameOverPopUp
    private void checkGameOver()
    {
        if(lives <= 0)
        {
            GameEvents.OnGameOverMethod();
            gameOverPopUp.SetActive(true);
        }
    }

    // if player makes mistake set one error image
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
        // subscribe to OnWrongColor event
        GameEvents.OnWrongColor += WrongColor;
    }

    private void OnDisable()
    {
        // unsubscribe to OnWrongColor event
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

