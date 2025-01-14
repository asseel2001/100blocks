using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]

public class SquareTextureData : ScriptableObject
{
    [System.Serializable]
    public class TextureData
    {
        public Sprite texture;
        public Config.SquareColor squareColor;
    }

    public int ThresholdVal=10;
    private const int StartTresholdVal = 100;
    public List<TextureData> ActiveSquareData = new List<TextureData>();

    public Config.SquareColor currentColor;
    private Config.SquareColor nextColor;

    private int GetCurrentColorIndex()
    {
        var CurrenIndex = 0;
        for(int i=0; i<ActiveSquareData.Count; i++)
        {
            if (ActiveSquareData[i].squareColor == currentColor)
            {
                CurrenIndex = i;
            }
                

        }
        return CurrenIndex;
    }

    public void UpdateColor(int Current_score)
    {
        currentColor = nextColor;
        var currentColorIndex = GetCurrentColorIndex();
        if(currentColorIndex==ActiveSquareData.Count-1)
        {
            nextColor = ActiveSquareData[0].squareColor;
        }
        else
        {
            nextColor = ActiveSquareData[currentColorIndex+1].squareColor;
        }
        ThresholdVal = StartTresholdVal + Current_score;
    }

    public void setStartColor()
    {
        ThresholdVal = StartTresholdVal;
        currentColor = ActiveSquareData[0].squareColor;
        nextColor = ActiveSquareData[1].squareColor;
    }

    private void Awake()
    {
        setStartColor();
    }

    private void OnEnable()
    {
        setStartColor();
    }
}


