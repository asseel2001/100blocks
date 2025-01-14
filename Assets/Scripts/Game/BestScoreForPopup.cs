using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BestScoreForPopup : MonoBehaviour
{
    public TMP_Text BestScoreText;

    private void OnEnable()
    {
        GameEvents.UpdateBestScoreBar += UpdateBestScoreBar;

    }

    private void OnDisable()
    {
        GameEvents.UpdateBestScoreBar -= UpdateBestScoreBar;
    }

    private void UpdateBestScoreBar(int currentScore, int BestScore)
    {
        float currentPercentage = (float)currentScore / (float)BestScore;
        
        BestScoreText.text = BestScore.ToString();
    }





}

