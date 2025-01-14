using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shape : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IDragHandler, IEndDragHandler, IBeginDragHandler

{
    public GameObject squareShapeImage;
     public Vector3 shapeSelectedScale;
     public Vector2 offset = new Vector2(0f, 700f);



      [HideInInspector]
      public ShapeData currentShapeData;

      public int TotalSquareNumber { get;  set; }

      private List<GameObject> _currentShape = new List<GameObject>();
      private Vector3 _shapeStartScale;
      private RectTransform _transform;
      private bool _shapeDraggable = true;
      private bool dragging = false;
      private Canvas _canvas;
    private Vector3 _StartPosition;
    private bool _shapeActive = true;

    



    public void Awake()
     {
         _shapeStartScale = this.GetComponent<RectTransform>().localScale;
         _transform = this.GetComponent<RectTransform>();
         _canvas = GetComponentInParent<Canvas>();
         _shapeDraggable = true;
        _StartPosition= transform.localPosition;
        _shapeActive = true;
     }

    private void OnEnable()
    {
        GameEvents.MoveShapeToStartPos += MoveShapeToStartPos;
        GameEvents.setShapeInActive += setShapeInactive;
    }

    private void OnDisable()
    {
        GameEvents.MoveShapeToStartPos -= MoveShapeToStartPos;
        GameEvents.setShapeInActive -= setShapeInactive;
    }
    public bool IsOnStartPosition()
    {
        return _transform.localPosition == _StartPosition;
    }


    //when you need to inactive shape after its down,
    //instead of doing all calculation to find which shape we
    //place to inactive you can simply create a public Shape
    //variable make hideInInspector in shape storage or use property,
    //and onpointerdown do shapeStorage.currentShape = this,
    //so then when you place the shape and want to inactive it,
    //you can simply call ShapeStorage.currentShape.DeactivateShape()
    //instead of ShapeStorage.GetCurrentSelectedShape(),
    //its more easier to read and less calculation,
    //since dont need to do any for loop or for each for
    //every shape on list
    public bool IsAnyOfShapeSquareActive()
    {

        foreach(var square in _currentShape)
        {
            if(square.gameObject.activeSelf)
            {
                return true;
            }

        }
        return false;

    }

    public void DeActivateShape()
    {
        if (_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().DeActivateShape();

            }
        }
        _shapeActive=false;

    }

    private void setShapeInactive()
    {
        if (IsOnStartPosition() == false && IsAnyOfShapeSquareActive())
        {
            foreach (var square in _currentShape)
            {
                square?.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateShape()
    {
        if (!_shapeActive)
        {
            foreach (var square in _currentShape)
            {
                square?.GetComponent<ShapeSquare>().ActivateShape();

            }
        }
        _shapeActive = true;

    }




    public void requestNewShape(ShapeData shapeData)
    {
        transform.localPosition = _StartPosition;
        CreateShape(shapeData);
    }

    public void CreateShape(ShapeData shapeData)
    {
        currentShapeData = shapeData;
        TotalSquareNumber=GetNumberOfSquares(shapeData); 
        
        while (_currentShape.Count <= TotalSquareNumber)
        {
            _currentShape.Add(Instantiate(squareShapeImage, transform) as GameObject);
        }

        foreach (var square in _currentShape)
        {
            square.gameObject.transform.position = Vector3.zero;
            square.gameObject.SetActive(false);
        }

        var squareRect = squareShapeImage.GetComponent<RectTransform>();
        var moveDistance = new Vector2(squareRect.rect.width * squareRect.localScale.x,
            squareRect.rect.height * squareRect.localScale.y);
        int currentIndexInList = 0;

        //to set position to form shape

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    _currentShape[currentIndexInList].SetActive(true);
                    _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition = new Vector2
                        (GetXPositionForShape(shapeData, column, moveDistance), GetYPositionForShape(shapeData, row, moveDistance));

                    currentIndexInList++;
                }
            }
        }

    }

    //to make distance between the square shapes
    private float GetYPositionForShape(ShapeData shapeData, int row, Vector2 moveDistance)
    {
        float shiftOnY = 0f;
        if (shapeData.rows > 1)
        {
            float startYPos;
            if (shapeData.rows % 2 != 0)
                startYPos = (shapeData.rows / 2) * moveDistance.y;
            else
                startYPos = ((shapeData.rows / 2) - 1) * moveDistance.y + moveDistance.y / 2;
            shiftOnY = startYPos - row * moveDistance.y;
        }
        return shiftOnY;
        /* float shiftOnY = 0f;
         if (shapeData.rows > 1)
         {
             if (shapeData.rows % 2 != 0)
             {
                 var middleSquareIndex = (shapeData.rows - 1) / 2;
                 var multiplier = (shapeData.rows - 1) / 2;
                 if (row < middleSquareIndex)
                 {
                     shiftOnY = moveDistance.y * -1;
                     shiftOnY *= multiplier;

                 }
                 else if (row > middleSquareIndex)
                 {
                     shiftOnY = moveDistance.y * 1;
                     shiftOnY *= multiplier;

                 }
             }
             else
             {
                 var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                 var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 1);
                 var multiplier = (shapeData.rows / 2);

                 if (row == middleSquareIndex1 || row == middleSquareIndex2)
                 {
                     if (row == middleSquareIndex2)
                         shiftOnY = moveDistance.y / 2;
                     if (row == middleSquareIndex1)
                         shiftOnY = (moveDistance.y / 2) * -1;
                 }

                 if (row < middleSquareIndex1 && row < middleSquareIndex2)
                 {
                     shiftOnY = -moveDistance.y * -1;
                     shiftOnY *= multiplier;
                 }
                 else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                 {
                     shiftOnY = -moveDistance.y * 1;
                     shiftOnY *= multiplier;
                 }
             }


         }
         return shiftOnY;*/

    }

    private float GetXPositionForShape(ShapeData shapeData, int column, Vector2 moveDistance)
    {
        //code doesnt work, or works with errors
        /*float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            if (shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;
                if (column < middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * -1;
                    shiftOnX *= multiplier;

                }
                else if (column > middleSquareIndex)
                {
                    shiftOnX = moveDistance.x * 1;
                    shiftOnX *= multiplier;

                }
            }
        }
        else
        {
            var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
            var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
            var multiplier = (shapeData.columns / 2);

            if (column == middleSquareIndex1 || column == middleSquareIndex2)
            {
                if (column == middleSquareIndex2)
                    shiftOnX = moveDistance.x / 2;
                if (column == middleSquareIndex1)
                    shiftOnX = (moveDistance.x / 2) * -1;
            }

            if (column < middleSquareIndex1 && column < middleSquareIndex2)
            {
                shiftOnX = -moveDistance.x * -1;
                shiftOnX *= multiplier;
            }
            else if (column > middleSquareIndex1 && column > middleSquareIndex2)
            {
                shiftOnX = -moveDistance.x * 1;
                shiftOnX *= multiplier;
            }
        }
        return shiftOnX;*/

        float shiftOnX = 0f;
        if (shapeData.columns > 1)
        {
            float startXPos;
            if (shapeData.columns % 2 != 0)
                startXPos = (shapeData.columns / 2) * moveDistance.x * -1;
            else
                startXPos = ((shapeData.columns / 2) - 1) * moveDistance.x * -1 - moveDistance.x / 2;
            shiftOnX = startXPos + column * moveDistance.x;

        }
        return shiftOnX;
    
}

    private int GetNumberOfSquares(ShapeData shapeData)
    {
        int num = 0;
        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                    num++;
            }
        }
        return num;

    }
    
    public void OnPointerClick(PointerEventData eventData)
    {

    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        _transform.anchorMin = new Vector2(0, 0);
        _transform.anchorMax = new Vector2(0, 0);
        _transform.pivot = new Vector2(0, 0);
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, eventData.position, Camera.main, out pos);
        _transform.localPosition = pos + offset;
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = _shapeStartScale;
        GameEvents.CheckIfShapeCanBePlaced();


    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().localScale = shapeSelectedScale;

    }

    private void MoveShapeToStartPos()
    {
        _transform.transform.localPosition = _StartPosition;

    }


}

