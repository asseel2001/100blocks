using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare : MonoBehaviour
{
    public Image HooverImage;
    public Image ActiveImage;
    public Image normalImage;
    public List<Sprite> normalImages;
    private Config.SquareColor CurrentSquareColor = Config.SquareColor.NotSet;

    public Config.SquareColor GetCurrentColor ()
    {
        return CurrentSquareColor;
    }

    public bool selected { get; set; }
    public int squareIndex { get; set; }
    public bool squareOccupied { get; set; }



    void Start()
    {

        selected = false;
        squareOccupied = false;


    }

    //temp function
    public bool CanWeUseThisSquare()
    {
        return HooverImage.gameObject.activeSelf;
    }

    public void PlaceShapeOnBoard(Config.SquareColor color)
    {
        CurrentSquareColor = color;
        ActivateSquare();
    }



    public void ActivateSquare()
    { 
        HooverImage.gameObject.SetActive(false);
        ActiveImage.gameObject.SetActive(true);
        selected = true;
        squareOccupied = true;
    }

    public void DeActivate()
    {
        CurrentSquareColor=Config.SquareColor.NotSet;
        ActiveImage.gameObject.SetActive(false);
    }

    public void ClearOccupied()
    {
        CurrentSquareColor = Config.SquareColor.NotSet;
        selected = false;
        squareOccupied = false;
    }

    public void SetImage(bool setFirstImage)
    {
        normalImage.GetComponent<Image>().sprite = setFirstImage? normalImages[1] : normalImages[0];
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(squareOccupied==false)
        {
            selected = true;
            HooverImage.gameObject.SetActive(true);

        }
        else if(collision.GetComponent<ShapeSquare>()!=null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        selected = true;
        if (squareOccupied == false)
        {
            
            HooverImage.gameObject.SetActive(true);
        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().SetOccupied();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (squareOccupied == false)
        {
            selected = false;
            HooverImage.gameObject.SetActive(false);

        }
        else if (collision.GetComponent<ShapeSquare>() != null)
        {
            collision.GetComponent<ShapeSquare>().unsetOccupied() ;
        }
            
    }


}
