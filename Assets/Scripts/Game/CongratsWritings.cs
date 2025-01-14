using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratsWritings : MonoBehaviour
{
    public List<GameObject> Writings;

    void Start()
    {
        GameEvents.ShowCongratsWritings += ShowCongrtsWritings;
    }

    private void OnDisable()
    {
        GameEvents.ShowCongratsWritings -= ShowCongrtsWritings;
    }

    private void ShowCongrtsWritings()
    {
        var index=UnityEngine.Random.Range(0, Writings.Count);
        Writings[index].SetActive(true);
    }

}
