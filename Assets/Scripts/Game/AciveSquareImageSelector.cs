using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AciveSquareImageSelector : MonoBehaviour
{
  public SquareTextureData squareTextureData;
    public bool updateImageOnREachThreshold = false;

    private void OnEnable()
    {
        UpdateSquareColorBasedOnCurrentPoints();
        if(updateImageOnREachThreshold)
        {
            GameEvents.UpdateSquareColor += UpdateSquareColor;
        }
    }

    private void OnDisable()
    {
        if (updateImageOnREachThreshold)
        {
            GameEvents.UpdateSquareColor -= UpdateSquareColor;
        }
    }

    private void UpdateSquareColorBasedOnCurrentPoints()
    {
        foreach(var squareTeaxture in squareTextureData.ActiveSquareData)
        {
            if(squareTextureData.currentColor==squareTeaxture.squareColor)
            {
                GetComponent<Image>().sprite = squareTeaxture.texture;

            }

        }
    }

    private void UpdateSquareColor(Config.SquareColor color)
    {
        foreach(var SquareTexture in squareTextureData.ActiveSquareData)
        {
            if(color== SquareTexture.squareColor)
            {
                GetComponent<Image>().sprite=SquareTexture.texture;
            }
        }
    }

}
