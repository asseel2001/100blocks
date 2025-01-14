using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class BestScoreData
{
    public int Score=0;
}

public class Scores : MonoBehaviour
{
    public SquareTextureData squareTextureData;
    public TMP_Text scoreText;
    private bool newBestScore = false;
    private BestScoreData bestScore=new BestScoreData();
    private int Curscore;
    private string BestScoreKey = "bsdat";
    
    private void Awake()
    {
        if(BinaryDataStream.Exist(BestScoreKey))
        {
            StartCoroutine(ReadDataFile());
        }
    }

    private IEnumerator ReadDataFile()
    {
        bestScore = BinaryDataStream.Read<BestScoreData>(BestScoreKey);
        yield return new WaitForEndOfFrame();
        GameEvents.UpdateBestScoreBar(Curscore, bestScore.Score);
    }

    void Start()
    {
        Curscore = 0;
        newBestScore = false;
        squareTextureData.setStartColor();
        UpdateScore();


    }

    

    private void OnEnable()
    {
        GameEvents.AddScore += Addscores;
        GameEvents.GameOver += saveBestScores;
    }

    private void OnDisable()
    {
        GameEvents.AddScore -= Addscores;
        GameEvents.GameOver -= saveBestScores;
    }

    public void saveBestScores(bool newBestScores)
    {
        BinaryDataStream.save<BestScoreData>(bestScore, BestScoreKey);
    }

    private void Addscores(int Scores)
    {
        Curscore += Scores;
        if(Curscore> bestScore.Score)
        {
            newBestScore = true;
            bestScore.Score = Curscore;
            saveBestScores(true);
        }
        updateSquareColor();
        GameEvents.UpdateBestScoreBar(Curscore, bestScore.Score);
        UpdateScore();


    }

    private void updateSquareColor()
    {
        if( GameEvents.UpdateSquareColor!=null && Curscore>=squareTextureData.ThresholdVal)
        {
            squareTextureData.UpdateColor(Curscore);
            GameEvents.UpdateSquareColor(squareTextureData.currentColor);
        }

    }

    private void UpdateScore()
    { 
        scoreText.text=Curscore.ToString();
    }
}
