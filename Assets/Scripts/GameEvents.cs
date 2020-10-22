using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void UpdateSquareColor(int number);
    public static event UpdateSquareColor OnUpdateSquareColor;

    public static void UpdateSquareColorMethod(int number)
    {
        if (OnUpdateSquareColor != null)
        {
            OnUpdateSquareColor(number);
        }
    }

    public delegate void SquareSelected(int square_index);
    public static event SquareSelected OnSquareSelected;

    public static void SquareSelectedMethod(int square_index)
    {
        if (OnSquareSelected != null)
            OnSquareSelected(square_index);
    }
}
