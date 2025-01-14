using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool> GameOver;

    public static Action<int> AddScore;

    public static Action CheckIfShapeCanBePlaced;

    public static Action MoveShapeToStartPos;

    public static Action RequestNewShape;

    public static Action setShapeInActive;

    public static Action<int, int> UpdateBestScoreBar;

    public static Action<Config.SquareColor> UpdateSquareColor;

    public static Action ShowCongratsWritings;

    public static Action<Config.SquareColor> ShowBonusScreen;
}
