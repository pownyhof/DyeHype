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
