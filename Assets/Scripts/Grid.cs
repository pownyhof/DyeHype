using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private int selectedLevel;
    private int playsFirstTime = 1;
    public int columns = 0;
    public int rows = 0;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public float square_gap = 0.1f;

    public Button jokerButton;
    public Button audioButton;
    public Sprite audioOn;
    public Sprite audioOff;
    public GameObject levelText;
    public GameObject rulesPopUp;
    public GameObject jokerPopUp;
    public AudioSource boxCompletedSound;

    private List<GameObject> grid_squares_ = new List<GameObject>();
    


    void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("selectedLevel");
        playsFirstTime = PlayerPrefs.GetInt("firstTime", 1);

        setAudioButton();
        setLevelText();
        CreateGrid();

        if (playsFirstTime == 1)
        {
            PlayerPlaysFirstTime();
        }

        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
            if(jokerUsed == 0)
            {
                jokerButton.GetComponentInChildren<Text>().text = "1";
            }
            else
            {
                jokerButton.GetComponentInChildren<Text>().text = "0";
            }
            
            SetGridFile();
        }
        else
        {
            jokerButton.GetComponentInChildren<Text>().text = "1";
            var data = GameData.Instance.dyehype_game[selectedLevel];
            SetGridNumbers(data);      
        }   
    }

    void SetGridFile()
    {
        selectedLevel = Config.ReadGameLevel();
        var data = Config.ReadGridData();

        SetGridNumbers(data);
    }

    private void OnEnable()
    {
        GameEvents.OnCheckGameCompleted += CheckGameCompleted;
        GameEvents.OnCheckBoxCompleted += OnCheckBoxCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnCheckGameCompleted -= CheckGameCompleted;
        GameEvents.OnCheckBoxCompleted -= OnCheckBoxCompleted;
        //----------------------------------------------------
        var solved_data = GameData.Instance.dyehype_game[selectedLevel].solved_data;
        int[] unsolved_data = new int[121];

        for(int i = 0; i < grid_squares_.Count; i++)
        {
            var comp = grid_squares_[i].GetComponent<GridSquare>();
            unsolved_data[i] = comp.GetSquareNumber();       
        }

        GameData.GameBoardData currentGame = new GameData.GameBoardData(unsolved_data, solved_data);

        // so data gets not saved when player finished a level
        if (GameSettings.Instance.GetExitAfterWon() == false)
        {
            Config.SaveBoardData(currentGame, selectedLevel, Lives.instance.GetErrorNumber());
        }
        else
        {
            Config.DeleteFile();
        }
    }


    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }


    private void SpawnGridSquares()
    {
        int square_index = 0;
        for(int row = 0; row < rows; row++)
        {
            for(int column = 0; column < columns; column++)
            {
                grid_squares_.Add(Instantiate(grid_square) as GameObject);
                grid_squares_[grid_squares_.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                // grid_squares_[grid_squares_.Count - 1].transform.parent = this.transform; // instantiate this game object as a child of the object holding this script.
                grid_squares_[grid_squares_.Count - 1].transform.SetParent(this.transform, false);
                grid_squares_[grid_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);

                square_index++;
            }
        }
    }


    // checks every time after player enters a square if the entered square completes a 2x2 Box so a different sound can be played
    private void OnCheckBoxCompleted(int squareIndex)
    {    
        Debug.Log("made it to the method");
        Debug.Log(squareIndex);

        bool firstSquareCorrect = false;
        bool secondSquareCorrect = false;

        // top rows of 2x2 boxes
        if ((squareIndex > 11 && squareIndex < 22) || (squareIndex > 33 && squareIndex < 44) || (squareIndex > 55 && squareIndex < 66) || (squareIndex > 77 && squareIndex < 88) || (squareIndex > 99 && squareIndex < 110))
            {
            Debug.Log("you are in a top row");
            // top left corner of 2x2 box
            if (squareIndex % 2 == 0)
            {
                Debug.Log("you are on the left");
                // iterate through all squares
                foreach (GameObject square in grid_squares_)
                {
                    Debug.Log(square.GetComponent<GridSquare>().GetDefaultValue());
                    // if top right corner is already filled
                    if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 1)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || firstSquareCorrect)
                    {
                        firstSquareCorrect = true;
                        // if bottom left corner is already filled
                        if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 11)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || secondSquareCorrect)
                        {
                            secondSquareCorrect = true;
                            // if bottom right corner is already filled
                            if ((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 12)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true))
                            {
                                Debug.Log("Play Sound!");
                                // play sound
                                boxCompletedSound.Play();
                                return;
                            }
                        }
                    }
                }
            }
            // top right corner of 2x2 box
            else
            {
                // iterate through all squares
                foreach (GameObject square in grid_squares_)
                {
                    // if top left corner is already filled
                    if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 1)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || firstSquareCorrect)
                    {
                        firstSquareCorrect = true;
                        // if bottom left corner is already filled
                        if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 10)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || secondSquareCorrect)
                        {
                            secondSquareCorrect = true;
                            // if bottom right corner is already filled
                            if ((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 11)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true))
                            {
                                Debug.Log("Play Sound!");
                                // play sound
                                boxCompletedSound.Play();
                                return;
                            }
                        }
                    }
                }
            }
                
            }
            // bottom rows of 2x2 boxes
            else
            {
            // bottom right corner of 2x2 box
            if (squareIndex % 2 == 0)
            {
                // iterate through all squares
                foreach (GameObject square in grid_squares_)
                {
                    // if top left corner is already filled
                    if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 12)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || firstSquareCorrect)
                    {
                        firstSquareCorrect = true;
                        // if top right corner is already filled
                        if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 11)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || secondSquareCorrect)
                        {
                            secondSquareCorrect = true;
                            // if bottom left corner is already filled
                            if ((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 1)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true))
                            {
                                Debug.Log("Play Sound!");
                                // play sound
                                boxCompletedSound.Play();
                                return;
                            }
                        }
                    }
                }
            }
            // bottom left corner of 2x2 box
            else
            {
                // iterate through all squares
                foreach (GameObject square in grid_squares_)
                {                 
                     // if top left corner is already filled
                     if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 11)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || firstSquareCorrect)
                     {
                        firstSquareCorrect = true;
                        // if top right corner is already filled
                        if (((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex - 10)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true)) || secondSquareCorrect)
                         {
                            secondSquareCorrect = true;
                            // if bottom right corner is already filled
                            if ((square.GetComponent<GridSquare>().GetSquareIndex() == (squareIndex + 1)) && (square.GetComponent<GridSquare>().GetDefaultValue() == true))
                            {
                                Debug.Log("Play Sound!");
                                // play sound
                                boxCompletedSound.Play();
                                return;
                            }
                         }
                     }
                }
            }
        }   
    }

    private void SetSquaresPosition()
    {
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();

        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool rowMoved = false;

        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + every_square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + every_square_offset;

        int column_number = 0;
        int row_number = 0;

        foreach (GameObject square in grid_squares_)
        {
            if(column_number + 1 > columns)
            {
                row_number++;
                column_number = 0;
                square_gap_number.x = 0;
                rowMoved = false;
            }

            var pos_x_offset = offset.x * column_number + (square_gap_number.x * square_gap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * square_gap);

            if(column_number > -1 && column_number % 2 != 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }
            if(row_number > -1 && row_number % 2 != 0 && rowMoved == false)
            {
                rowMoved = true;
                square_gap_number.y++;
                pos_y_offset += square_gap;
            }
            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(start_position.x + pos_x_offset, start_position.y - pos_y_offset);
            column_number++;
        }
    }

    private void SetGridNumbers(GameData.GameBoardData data)
    {

        for (int index = 0; index < grid_squares_.Count; index++)
        {

            grid_squares_[index].GetComponent<GridSquare>().SetNumber(data.unsolved_data[index]);
            grid_squares_[index].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[index]);
       
            // prevents player from changing outside clue squares and already correct squares
            grid_squares_[index].GetComponent<GridSquare>().SetDefaultValue((index % 11 == 0) || (index < 11) || (data.unsolved_data[index] != 0 && data.unsolved_data[index] == data.solved_data[index]));
            
        }
    }

    private void setAudioButton()
    {
        int audioMuted = PlayerPrefs.GetInt("audioMuted", 0);
        if (audioMuted == 0)
        {        
            audioButton.GetComponent<Image>().sprite = audioOn;
        }
        else
        {         
            audioButton.GetComponent<Image>().sprite = audioOff;
        }
    }

    private void setLevelText()
    {
        int currentLevel = selectedLevel + 1;
        levelText.GetComponent<Text>().text = "Level " + currentLevel.ToString();
    }

    private void PlayerPlaysFirstTime()
    {
        rulesPopUp.SetActive(true);
        PlayerPrefs.SetInt("firstTime", 0);
        Invoke("showJokerButton", 100);
    }

    private void showJokerButton()
    {
        jokerPopUp.SetActive(true);
    }

    private void CheckGameCompleted()
    {
        // gets called every time player enters a square
        foreach(var square in grid_squares_)
        {
            var comp = square.GetComponent<GridSquare>();
            // if one square is still wrong player has to continue game
            if(comp.IsCorrectSquareSet() == false)
            {
                return;
            }
        }
        // gets called when player has entered all squares correctly
        GameEvents.OnGameCompletedMethod();
    }
}
