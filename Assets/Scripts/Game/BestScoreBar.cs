using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BestScoreBar : MonoBehaviour
{
    public Image FillInImage;
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
        float currentPercentage=(float)currentScore/(float)BestScore;
        FillInImage.fillAmount=currentPercentage;
        BestScoreText.text = BestScore.ToString();
    }





}
