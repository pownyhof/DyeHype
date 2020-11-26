using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    // variables
    private int selectedLevel;
    private int hitsLevelSix;
    private int hitsLevelTen;
    private int playsFirstTime = 1;
    public int columns = 0;
    public int rows = 0;
    public float every_square_offset = 0.0f;
    public Vector2 start_position = new Vector2(0.0f, 0.0f);
    public float square_scale = 1.0f;
    public float square_gap = 0.1f;

    // objects
    public Sprite audioOn;
    public Sprite audioOff;
    public Button jokerButton;
    public Button audioButton;
    public GameObject nextButtonOne;
    public GameObject nextButtonTwo;
    public GameObject grid_square;
    public GameObject levelText;
    public GameObject rulesPopUp;
    public GameObject rulesPopUpTwo;
    public GameObject rulesPopUpThree;
    public GameObject jokerPopUp;
    public AudioSource boxCompletedSound;

    // rules text depending on selected language
    string[] rules;
    string[] joker;
    // first RulesPopUp
    public Text topText;
    public Text midText;
    public Text bottomText;
    public Text jokerText;
    // second RulesPopUp
    public Text topTextTwo;
    public Text bottomTextTwo;
    // thirs RulesPopUp
    public Text topTextThree;
    public Text midTextThree;
    public Text bottomTextThree;

    private List<GameObject> grid_squares_ = new List<GameObject>();

    void Start()
    {
        // check for example is user is playing the first time or has hit a certain level
        checkPlayPrefs();
        // init audio icon in top panel depending on player muted game or not
        setAudioButton();
        // tiny text on the bottom left depending on level user is playing
        setLevelText();
        // set rules and joker text
        SetRulesText();
        CreateGrid();

        if (GameSettings.Instance.GetContinuePreviousGame())
        {
            int jokerUsed = PlayerPrefs.GetInt("jokerUsed");
            if (jokerUsed == 0)
            {
                jokerButton.GetComponentInChildren<Text>().text = "1";
            }
            else
            {
                jokerButton.GetComponentInChildren<Text>().text = "0";
            }
            // call method to load gameData from gameData.ini if player continued a game
            SetGridFile();
        }
        else
        {
            // else load data from selectedLevel
            jokerButton.GetComponentInChildren<Text>().text = "1";
            var data = GameData.Instance.dyehype_game[selectedLevel];
            SetGridNumbers(data);
        }

        AdManager.Instance.HideBanner();
        AdManager.Instance.ShowInterstitialAd();

    }

    private void checkPlayPrefs()
    {
        selectedLevel = PlayerPrefs.GetInt("selectedLevel");
        playsFirstTime = PlayerPrefs.GetInt("firstTime", 1);
        hitsLevelSix = PlayerPrefs.GetInt("hitsLevelSix", 1);
        hitsLevelTen = PlayerPrefs.GetInt("hitsLevelTen", 1);

        if (playsFirstTime == 1)
        {
            PlayerPlaysFirstTime();
        }
        if ((hitsLevelSix == 1) && (PlayerPrefs.GetInt("levelReached") == 5))
        {
            PlayerHitsLevelSix();
        }
        if ((hitsLevelTen == 1) && (PlayerPrefs.GetInt("levelReached") == 9))
        {
            PlayerHitsLevelTen();
        }
        if (PlayerPrefs.GetInt("levelReached") >= 5)
        {
            nextButtonOne.SetActive(true);
        }
        if (PlayerPrefs.GetInt("levelReached") >= 9)
        {
            nextButtonTwo.SetActive(true);
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
        // subscribe to events
        GameEvents.OnCheckGameCompleted += CheckGameCompleted;
        GameEvents.OnCheckBoxCompleted += OnCheckBoxCompleted;
        GameEvents.OnSetJokerText += OnSetJokerText;
    }

    private void OnDisable()
    {
        // unsubscribe to events
        GameEvents.OnCheckGameCompleted -= CheckGameCompleted;
        GameEvents.OnCheckBoxCompleted -= OnCheckBoxCompleted;
        GameEvents.OnSetJokerText -= OnSetJokerText;
        //----------------------------------------------------
        // save data to gameData.ini when player leaves game and has level not yet completed
        var clues_data = GameData.Instance.dyehype_game[selectedLevel].clues_data;
        var solved_data = GameData.Instance.dyehype_game[selectedLevel].solved_data;
        int[] unsolved_data = new int[121];

        for (int i = 0; i < grid_squares_.Count; i++)
        {
            var comp = grid_squares_[i].GetComponent<GridSquare>();
            unsolved_data[i] = comp.GetSquareNumber();
        }

        GameData.GameBoardData currentGame = new GameData.GameBoardData(clues_data, unsolved_data, solved_data);

        // so data gets not saved when player finished a level
        if (GameSettings.Instance.GetExitAfterWon() == false)
        {
            Config.SaveBoardData(currentGame, selectedLevel, Lives.instance.GetErrorNumber());
        }
        else
        {
            Config.DeleteFile();
        }

        AdManager.Instance.HideBanner();
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }


    private void SpawnGridSquares()
    {
        int square_index = 0;
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // create a gridSquare Object
                grid_squares_.Add(Instantiate(grid_square) as GameObject);
                grid_squares_[grid_squares_.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                // grid_squares_[grid_squares_.Count - 1].transform.parent = this.transform; // instantiate this game object as a child of the object holding this script.
                grid_squares_[grid_squares_.Count - 1].transform.SetParent(this.transform, false); // according to unity better to use this method that the one above
                grid_squares_[grid_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);

                square_index++;
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
            if (column_number + 1 > columns)
            {
                row_number++;
                column_number = 0;
                square_gap_number.x = 0;
                rowMoved = false;
            }

            var pos_x_offset = offset.x * column_number + (square_gap_number.x * square_gap);
            var pos_y_offset = offset.y * row_number + (square_gap_number.y * square_gap);

            // needed so every second 'gap line' is bigger, so player can better recognize the 2x2 big boxes
            if (column_number > -1 && column_number % 2 != 0)
            {
                square_gap_number.x++;
                pos_x_offset += square_gap;
            }
            if (row_number > -1 && row_number % 2 != 0 && rowMoved == false)
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
            // set clues inside grid, set starting number/sprite and set solution number/sprite
            grid_squares_[index].GetComponent<GridSquare>().SetClue(data.clues_data[index]);
            grid_squares_[index].GetComponent<GridSquare>().SetNumber(data.unsolved_data[index]);
            grid_squares_[index].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[index]);

            // prevents player from changing outside clue squares and already correct squares
            grid_squares_[index].GetComponent<GridSquare>().SetDefaultValue((index % 11 == 0) || (index < 11) || (data.unsolved_data[index] != 0 && data.unsolved_data[index] == data.solved_data[index]));

        }
    }

    private void setAudioButton()
    {
        // init correct audio button depending on player muted game or not
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

    // sets text and rules depending on language that player chose
    public void SetRulesText()
    {
        joker = Language.Instance.GetJokerClue();
        // check which language player has selected
        if (PlayerPrefs.GetInt("language") == 0)
        {
            rules = Language.Instance.GetRules(0);
            jokerText.text = joker[0];
        }
        if (PlayerPrefs.GetInt("language") == 1)
        {
            rules = Language.Instance.GetRules(1);
            jokerText.text = joker[1];
        }
        if (PlayerPrefs.GetInt("language") == 2)
        {
            rules = Language.Instance.GetRules(2);
            jokerText.text = joker[2];
        }
          
        // set text fields of the different RulesPopUps
        topText.text = rules[0];
        midText.text = rules[1];
        bottomText.text = rules[2];
        topTextTwo.text = rules[3];
        bottomTextTwo.text = rules[4];
        topTextThree.text = rules[5];
        midTextThree.text = rules[6];
        bottomTextThree.text = rules[7];
    }

    // if it's users first time playing activate rulesPopUp automatically
    private void PlayerPlaysFirstTime()
    {
        rulesPopUp.SetActive(true);
        // save in player prefs firstTime to false, so next time it wont show up automatically
        PlayerPrefs.SetInt("firstTime", 0);
        // direct users attention to joker option
        Invoke("showJokerButton", 80);
    }

    private void PlayerHitsLevelSix()
    {
        rulesPopUpTwo.SetActive(true);
        // save in player prefs firstTime to false, so next time it wont show up automatically
        PlayerPrefs.SetInt("hitsLevelSix", 0);
    }

    private void PlayerHitsLevelTen()
    {
        rulesPopUpThree.SetActive(true);
        // save in player prefs firstTime to false, so next time it wont show up automatically
        PlayerPrefs.SetInt("hitsLevelTen", 0);
    }

    private void showJokerButton()
    {
        jokerPopUp.SetActive(true);
    }

    public void OnSetJokerText()
    {
        jokerButton.GetComponentInChildren<Text>().text = "0";
    }

    private void CheckGameCompleted()
    {
        // gets called every time player enters a square
        foreach (var square in grid_squares_)
        {
            var comp = square.GetComponent<GridSquare>();
            // if one square is still wrong player has to continue game
            if (comp.IsCorrectSquareSet() == false)
            {
                return;
            }
        }
        // gets called when player has entered all squares correctly
        GameEvents.OnGameCompletedMethod();
        AdManager.Instance.ShowBanner();
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
}
