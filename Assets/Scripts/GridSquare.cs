using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{

    public GameObject number_text;
    public GameObject clue;
    public GameObject grid_sprite;
    public GameObject highlightImage;
    public AudioSource rightSquareEntered;
    public AudioSource wrongSquareEntered;

    private int number_ = 0;
    private int correctColor = 0;
    private bool selected_ = false;
    private int square_index_ = -1;
    private bool SquareDefaultValue = false;

    public Sprite blue;
    public Sprite green;
    public Sprite yellow;
    public Sprite red;
    // sprites for clues
    public Sprite blue1;
    public Sprite blue2;
    public Sprite blue3;
    public Sprite blue4;
    public Sprite blue5;
    public Sprite green1;
    public Sprite green2;
    public Sprite green3;
    public Sprite green4;
    public Sprite green5;
    public Sprite yellow1;
    public Sprite yellow2;
    public Sprite yellow3;
    public Sprite yellow4;
    public Sprite yellow5;
    public Sprite red1;
    public Sprite red2;
    public Sprite red3;
    public Sprite red4;
    public Sprite red5;

    public Sprite defaultSquare;
    public Sprite logo;

   
    void Start()
    {
        // no square selected at the start of the game
        selected_ = false;
        // joker not yet used
        if (!GameSettings.Instance.GetContinuePreviousGame())
        {
            PlayerPrefs.SetInt("jokerUsed", 0);
        }
    }

    public void DisplayText()
    {
        SpriteRenderer rend = grid_sprite.GetComponent<SpriteRenderer>();
        // set sprite according to selected GameData
        switch (number_)
        {
            case 1: rend.sprite = blue; break;
            case 2: rend.sprite = green; break;
            case 3: rend.sprite = yellow; break;
            case 4: rend.sprite = red; break;
            case 5: rend.sprite = blue1; break;
            case 6: rend.sprite = blue2; break;
            case 7: rend.sprite = blue3; break;
            case 8: rend.sprite = blue4; break;
            case 9: rend.sprite = blue5; break;
            case 10: rend.sprite = green1; break;
            case 11: rend.sprite = green2; break;
            case 12: rend.sprite = green3; break;
            case 13: rend.sprite = green4; break;
            case 14: rend.sprite = green5; break;
            case 15: rend.sprite = yellow1; break;
            case 16: rend.sprite = yellow2; break;
            case 17: rend.sprite = yellow3; break;
            case 18: rend.sprite = yellow4; break;
            case 19: rend.sprite = yellow5; break;
            case 20: rend.sprite = red1; break;
            case 21: rend.sprite = red2; break;
            case 22: rend.sprite = red3; break;
            case 23: rend.sprite = red4; break;
            case 24: rend.sprite = red5; break;
            case 25: rend.sprite = logo; break;
            default: break;
        }
        if (number_ <= 0)
            number_text.GetComponent<Text>().text = " ";
        else
            number_text.GetComponent<Text>().text = number_.ToString();
    }

    public void SetClue(string clueString)
    {
        // set clues inside the grid according to selected GameData
        clue.GetComponent<Text>().text = clueString;
        if (clueString != "-")
        {
            clue.SetActive(true);
        }
    }


    public void SetNumber(int number)
    {
        // behind every sprite is a number in an int array, so later it can be easy checked if player set correct number/sprite and if player has set all numbersor sprites correct/has won the game
        number_ = number;
        DisplayText();
    }

    // called when player selects a square
    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true;
        highlightImage.SetActive(true);
        GameEvents.SquareSelectedMethod(square_index_);
    }


    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable()
    {
        // subscribe to events
        GameEvents.OnUpdateSquareColor += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
        GameEvents.OnClearSquare += OnClearSquare;
        GameEvents.OnJokerUsed += OnJokerUsed;
    }

    private void OnDisable()
    {
        // unsubscribe to events
        GameEvents.OnUpdateSquareColor -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
        GameEvents.OnClearSquare -= OnClearSquare;
        GameEvents.OnJokerUsed -= OnJokerUsed;
    }

    // so player can delete wrong entered squares
    public void OnClearSquare()
    {
        SpriteRenderer rend = grid_sprite.GetComponent<SpriteRenderer>();

        // player has selected square that has wrong value
        if (selected_ && !SquareDefaultValue)
        {
            // set back square value
            number_ = 0;
            // set back the square image
            rend.sprite = defaultSquare;
            // show changes
            DisplayText();
        }
    }

    public void OnSetNumber(int number)
    {
        if (selected_ && SquareDefaultValue == false)
        {
            SetNumber(number);

            if (number_ != correctColor)
            {

                // if player didnt mute audio, enter square sound gets played
                int audioMuted = PlayerPrefs.GetInt("audioMuted");
                if (audioMuted == 0)
                {
                    wrongSquareEntered.Play();
                }
                // changes Color to red if player enters wrong value
                var colors = this.colors;
                colors.normalColor = Color.red;
                this.colors = colors;

                GameEvents.OnWrongColorMethod();

                #region logging
                string eventType = "CLICK";
                string enteredColor = "";
                string playerId = PlayfabManager.Instance.GetUserID();
                int levelNumber = PlayerPrefs.GetInt("selectedLevel") + 1;
                string level = levelNumber.ToString();
                string field = square_index_.ToString();
                string correct = "No";
                string clickTime = Timer.GetCurrentTime();

                if (number_ == 1)
                {
                    enteredColor = "blue";
                }
                if (number_ == 2)
                {
                    enteredColor = "green";
                }
                if (number_ == 3)
                {
                    enteredColor = "yellow";
                }
                if (number_ == 4)
                {
                    enteredColor = "red";
                }

                LogOutputHandler.Instance.HandleClickLog(eventType, playerId, level, field, enteredColor, correct, clickTime);
                #endregion
            }
            else
            {
                // prevents player from changing already correct entered Squares or Squares already existing in starting Grid 
                SquareDefaultValue = true;

                // if player didnt mute audio, enter square sound gets played
                int audioMuted = PlayerPrefs.GetInt("audioMuted");
                if (audioMuted == 0)
                {
                    rightSquareEntered.Play();

                    // plays additionally another sound when player finished a 2x2 box
                    GameEvents.OnCheckBoxCompletedMethod(square_index_);
                }

                // changes color back to white if value is correct
                var colors = this.colors;
                colors.normalColor = Color.white;
                this.colors = colors;

                #region logging2
                string eventType = "CLICK";
                string enteredColor = "";
                string playerId = PlayfabManager.Instance.GetUserID();
                int levelNumber = PlayerPrefs.GetInt("selectedLevel") + 1;
                string level = levelNumber.ToString();
                string field = square_index_.ToString();
                string correct = "Yes";
                string clickTime = Timer.GetCurrentTime();

                if (number_ == 1)
                {
                    enteredColor = "blue";
                }
                if (number_ == 2)
                {
                    enteredColor = "green";
                }
                if (number_ == 3)
                {
                    enteredColor = "yellow";
                }
                if (number_ == 4)
                {
                    enteredColor = "red";
                }

                LogOutputHandler.Instance.HandleClickLog(eventType, playerId, level, field, enteredColor, correct, clickTime);
                #endregion
            }

            // check after every entered square if player solved the entire level
            GameEvents.CheckGameCompletedMethod();
        }
    }

    private void OnJokerUsed()
    {
        // joker sets correct color once for the player
        if (selected_ && SquareDefaultValue == false)
        {
            OnSetNumber(correctColor);
            PlayerPrefs.SetInt("jokerUsed", 1);
            GameEvents.OnSetJokerTextMethod();
        }      
    }

    public void OnSquareSelected(int square_index)
    {
        // is square is no longer selected disable highlighting
        if (square_index_ != square_index)
        {
            selected_ = false;
            highlightImage.SetActive(false);
        }
    }

    // setters and getters
    public void SetDefaultValue(bool defaultValue)
    {
        SquareDefaultValue = defaultValue;
    }

    public bool GetDefaultValue()
    {
        return SquareDefaultValue;
    }

    public bool IsSelected()
    {
        return selected_;
    }

    public void SetSquareIndex(int index)
    {
        square_index_ = index;
    }

    public int GetSquareIndex()
    {
        return square_index_;
    }

    public int GetSquareNumber()
    {
        return number_;
    }

    public void SetCorrectNumber(int number)
    {
        correctColor = number;
    }

    public bool IsCorrectSquareSet()
    {
        return number_ == correctColor;
    }
}

