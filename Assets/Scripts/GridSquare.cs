using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GridSquare : Selectable, IPointerClickHandler, ISubmitHandler, IPointerUpHandler, IPointerExitHandler
{

    public GameObject number_text;
    public GameObject grid_sprite;
    private int number_ = 0;
    private bool selected_ = false;
    private int square_index_ = -1;


    // #1F456E
    public Sprite blue;
    // #5da05a
    public Sprite green; 
    // #fbec5d
    public Sprite yellow; 
    // #be213e
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

    public bool IsSelected()
    {
        return selected_;
    }

    public void SetSquareIndex(int index)
    {
        square_index_ = index;
    }


    void Start()
    {
        selected_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayText()
    {
        SpriteRenderer rend = grid_sprite.GetComponent<SpriteRenderer>();

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
            default: break;
        }
        if(number_ <= 0)
            number_text.GetComponent<Text>().text = " ";
        else
            number_text.GetComponent<Text>().text = number_.ToString();
    }

    public void SetNumber(int number)
    {
        number_ = number;
        DisplayText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selected_ = true;
        GameEvents.SquareSelectedMethod(square_index_);
    }


    public void OnSubmit(BaseEventData eventData)
    {

    }

    private void OnEnable()
    {
        GameEvents.OnUpdateSquareColor += OnSetNumber;
        GameEvents.OnSquareSelected += OnSquareSelected;
    }

    private void OnDisable()
    {
        GameEvents.OnUpdateSquareColor -= OnSetNumber;
        GameEvents.OnSquareSelected -= OnSquareSelected;
    }

    public void OnSetNumber(int number)
    {
        if (selected_)
        {
            SetNumber(number);
        }
    }


    public void OnSquareSelected(int square_index)
    {
        if(square_index_ != square_index)
        {
            selected_ = false;
        }
    }
}
