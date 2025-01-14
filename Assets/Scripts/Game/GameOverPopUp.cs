using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    public GameObject GameOverPopup;
    public GameObject LosePopup;
    public GameObject NewBestScorePopup;

    void Start()
    {
        GameOverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += onGameover;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= onGameover;
    }

    private void onGameover(bool NewbestScore)
    {
        GameOverPopup.SetActive(true);
        LosePopup.SetActive(true);
        NewBestScorePopup.SetActive(true);
    }

}
