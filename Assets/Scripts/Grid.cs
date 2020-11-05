using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    private int selectedLevel;
    public int columns = 0;
    public int rows = 0;
    public float every_square_offset = 0.0f;
    public GameObject grid_square;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;

    public Button audioButton;
    public Sprite audioOn;
    public Sprite audioOff;
    public GameObject levelText;

    private List<GameObject> grid_squares_ = new List<GameObject>();
    


    void Start()
    {
        selectedLevel = PlayerPrefs.GetInt("selectedLevel");
        setAudioButton();
        setLevelText();
        CreateGrid();

        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            SetGridFile();
        }
        else
        {
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
    }

    private void OnDisable()
    {
        GameEvents.OnCheckGameCompleted -= CheckGameCompleted;
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

    private void SetSquaresPosition()
    {
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
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
            }

            var pos_x_offset = offset.x * column_number;
            var pos_y_offset = offset.y * row_number;
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
