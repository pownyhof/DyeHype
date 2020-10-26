using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public delegate void CheckGameCompleted();
    public static event CheckGameCompleted OnCheckGameCompleted;

    public static void CheckGameCompletedMethod()
    {
        if (OnCheckGameCompleted != null)
            OnCheckGameCompleted();
    }

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

    public delegate void WrongColor();
    public static event WrongColor OnWrongColor;

    public static void OnWrongColorMethod()
    {
        if (OnWrongColor != null)
            OnWrongColor();
    }

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public static void OnGameOverMethod()
    {
        if (OnGameOver != null)
            OnGameOver();
    }

    public delegate void clearSquare();
    public static event clearSquare OnClearSquare;

    public static void OnClearSquareMethod()
    {
        if (OnClearSquare != null)
            OnClearSquare();
    }

    public delegate void GameCompleted();
    public static event GameCompleted OnGameCompleted;

    public static void OnGameCompletedMethod()
    {
        if (OnGameCompleted != null)
            OnGameCompleted();
    }
     
}
