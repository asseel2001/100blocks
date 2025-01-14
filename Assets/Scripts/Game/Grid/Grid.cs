using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    //start vars
    public ShapeStorage ShapeStorage;
    public int columns = 0;
    public int rows = 0;
    public float squaresGap = 0.1f;
    public GameObject gridSquare;
    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    public float squareScale = 0.5f;
    public float everySquareOffset = 0.0f;
    public SquareTextureData squareTextureData;

    private Vector2 _offset = new Vector2(0.0f, 0.0f);

    private List<GameObject> _gridSquares = new List<GameObject>();

    private LineIndecator _lineIndecator;
    private Config.SquareColor _CurrentAciveSquareColor = Config.SquareColor.NotSet;
    private List<Config.SquareColor> ColorsInTheGrid=new List<Config.SquareColor>();


    //start funcs
    private void OnEnable()
    {
        GameEvents.CheckIfShapeCanBePlaced += CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor += OnUpdateSquareColor;


    }

    private void OnDisable()
    {
        GameEvents.CheckIfShapeCanBePlaced -= CheckIfShapeCanBePlaced;
        GameEvents.UpdateSquareColor -= OnUpdateSquareColor;

    }
    
    void Start()
    {
        _lineIndecator = GetComponent<LineIndecator>();
        CreateGrid();
        _CurrentAciveSquareColor = squareTextureData.ActiveSquareData[0].squareColor;
    }

    private void OnUpdateSquareColor(Config.SquareColor color)
    {
        _CurrentAciveSquareColor = color;
    }

    private List<Config.SquareColor> GetAllColorsInTheGrid()
    {
        var colors =new List<Config.SquareColor>();
        foreach(var square in _gridSquares)
        {
            var gridSquare = square.GetComponent<GridSquare>();
            if(gridSquare.squareOccupied)
            {
                var color = gridSquare.GetCurrentColor();
                if(colors.Contains(color)==false)
                {
                    colors.Add(color);
                }
            }
        }
        return colors;
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetGridSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        int square_index = 0;


        for (var row = 0; row < rows; ++row)
        {
            for (var col = 0; col < columns; ++col)
            {
                _gridSquares.Add(Instantiate(gridSquare) as GameObject);

                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().squareIndex = square_index;
                _gridSquares[_gridSquares.Count - 1].transform.SetParent(this.transform);
                _gridSquares[_gridSquares.Count - 1].transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                _gridSquares[_gridSquares.Count - 1].GetComponent<GridSquare>().SetImage(_lineIndecator.GetGridSquareIndex(square_index) % 2 == 0);

                square_index++;

            }
        }
    }

    private void SetGridSquaresPosition()
    {
        int column_num = 0;
        int row_num = 0;
        Vector2 square_gap_number = new Vector2(0.0f, 0.0f);
        bool row_moved = false;

        var square_rect = _gridSquares[0].GetComponent<RectTransform>();

        _offset.x = square_rect.rect.width * square_rect.transform.localScale.x + everySquareOffset;
        _offset.y = square_rect.rect.height * square_rect.transform.localScale.y + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (column_num + 1 > columns)
            {
                square_gap_number.x = 0;
                column_num = 0;
                row_num++;
                row_moved = false;
            }
            var pos_x_offset = _offset.x * column_num + (square_gap_number.x * squaresGap);
            var pos_y_offset = _offset.y * row_num + (square_gap_number.y * squaresGap);

            if (column_num > 0 && column_num % 3 == 0)
            {
                square_gap_number.x++;
                pos_x_offset += squaresGap;

            }

            if (row_num > 0 && row_num % 3 == 0 && row_moved == false)
            {
                row_moved = true;
                square_gap_number.y++;
                pos_y_offset += squaresGap;


            }

            square.GetComponent<RectTransform>().anchoredPosition = new Vector2(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset);

            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + pos_x_offset, startPosition.y - pos_y_offset, 0.0f);

            column_num++;

        }
    }

    private void CheckIfShapeCanBePlaced()
    {
        var squareIndexes=new List<int>();
        foreach (var square in _gridSquares)
        { 
            var gridsquare=square.GetComponent<GridSquare>();
            if (gridsquare.selected && !gridsquare.squareOccupied)
            { 
                squareIndexes.Add(gridsquare.squareIndex);
                gridsquare.selected = false;
                //gridsquare.ActivateSquare();

            }

        }
        var currentSelectedShape = ShapeStorage.GetCurrentShape();
        
        if(currentSelectedShape == null) 
            return; //no selected shape

        if (currentSelectedShape.TotalSquareNumber == squareIndexes.Count)
        {
            foreach (var squareIndex in squareIndexes)
            {
                _gridSquares[squareIndex].GetComponent<GridSquare>().PlaceShapeOnBoard(_CurrentAciveSquareColor);
            }
            var shapeLeft = 0;

            foreach(var shape in ShapeStorage.shapeList)
            {
                if (shape.IsOnStartPosition() && shape.IsAnyOfShapeSquareActive())
                {
                    shapeLeft++;
                }
            }
            

            if (shapeLeft==0)
            {

                GameEvents.RequestNewShape();
            }
            else 
            { 
                GameEvents.setShapeInActive();
            }
            

        }
        else 
        {
            GameEvents.MoveShapeToStartPos();
        }

        CheckIfAnyCompletedLine();


    }

    void CheckIfAnyCompletedLine()
    {
        List<int[]> lines = new List<int[]>();
        //columns
        foreach(var column in _lineIndecator.Columnline_Data)
        {
            lines.Add(_lineIndecator.GetVerticalLine(column));

        }
        //rows
        for (var row=0;row<9;row++)
        {
            List<int> data = new List<int>(9);
            for(var index=0; index<9; index++)
            {
                data.Add(_lineIndecator.line_Data[row,index]);
            }
            lines.Add(data.ToArray());
        }

        // squares 
        for (var square = 0; square < 9; square++)
        {
            List<int> data = new List<int>(9);
            for(var index=0;index<9; index++)
            {
                data.Add(_lineIndecator.Square_Data[square, index]);
            }
            lines.Add(data.ToArray());
        }

        //function needs to be called before checking if lines are complete
        ColorsInTheGrid = GetAllColorsInTheGrid();

        var completedLines=CheckIfSquaresCompleted(lines);
        if (completedLines >= 2)
        {
            //bonus animation
            GameEvents.ShowCongratsWritings();
        }

        //add scores
        var totalScores = 10 * completedLines;
        var bonusScores = shouldPlayColorBonusAnimation();
        GameEvents.AddScore(totalScores+bonusScores);
        CheckIfPlayerLost();
        
    }

    private int shouldPlayColorBonusAnimation()
    {
        var colorsInTheGridAfterLineRemoval = GetAllColorsInTheGrid();
        Config.SquareColor colorBonusPlay = Config.SquareColor.NotSet;
        foreach(var squareColor in ColorsInTheGrid)
        {
            if(colorsInTheGridAfterLineRemoval.Contains(squareColor)==false)
            {
                colorBonusPlay = squareColor;
            }
        }
        if(colorBonusPlay== Config.SquareColor.NotSet)
        {
            Debug.Log("cannot find color for the bonus");
            return 0;
        }
        //shoudnt never play bonus for current color
        if (colorBonusPlay == _CurrentAciveSquareColor)
        {
            
            return 0;
        }
        GameEvents.ShowBonusScreen(colorBonusPlay);
        return 50;


    }

    private int CheckIfSquaresCompleted(List<int[]> data)
    {
        List<int[]> completedLines = new List<int[]>();

        var linesCompleted = 0;
        foreach(var line in data)
        {
            var lineCompleted = true;
            foreach(var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                if(comp.squareOccupied==false)
                {
                    lineCompleted = false;
                }
            }

            if(lineCompleted)
            {
                completedLines.Add(line);
            }
        }
        foreach (var line in completedLines)
        {
            var completed = false;
            foreach (var squareIndex in line)
            { 
                var comp= _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.DeActivate();
                completed = true;
            }
            foreach (var squareIndex in line)
            {
                var comp = _gridSquares[squareIndex].GetComponent<GridSquare>();
                comp.ClearOccupied();
            }
            if(completed)
            {
                linesCompleted++;
            }
        }
        return linesCompleted;
    }

    private void CheckIfPlayerLost()
    {
        var validShapes = 0;

        for (var index=0; index<ShapeStorage.shapeList.Count; index++)
        {
            var isShapeActive = ShapeStorage.shapeList[index].IsAnyOfShapeSquareActive();
            if (CheckIfShapeCanBePlacedOnGrid(ShapeStorage.shapeList[index]) && isShapeActive)
            {
                ShapeStorage.shapeList[index]?.ActivateShape();
                validShapes++;
            }


        }
        if(validShapes==0)
        {
            GameEvents.GameOver(false);
            //Debug.Log("lost game");
        }

    }

    private bool CheckIfShapeCanBePlacedOnGrid(Shape curshape)
    {
        var currentShapeData = curshape.currentShapeData;
        var shapeCols = currentShapeData.columns;
        var shapeRows = currentShapeData.rows;

        //will contain all indexes of filled up squares
        List<int> originalShapeFilledUpSquares=new List<int>();
        var squareIndex = 0;
        for(var rowIndex=0; rowIndex<shapeRows; rowIndex++)
        {
            for(var columnIndex=0; columnIndex<shapeCols; columnIndex++)
            {
                if (currentShapeData.board[rowIndex].column[columnIndex])
                {
                    originalShapeFilledUpSquares.Add(squareIndex);
                }
                squareIndex++;
            }


        }
        if (curshape.TotalSquareNumber!= originalShapeFilledUpSquares.Count)
        {
            Debug.LogError("number is wrong");
        }

        var squareList = GetAllSquareesCombination(shapeCols, shapeRows);

        bool canBePlaced = false;
        foreach(var num in squareList)
        {
            bool shapeCanBePlacedOnBoard = true;
            foreach(var squareIndexToCheck in originalShapeFilledUpSquares)
            {
                var comp = _gridSquares[num[squareIndexToCheck]].GetComponent<GridSquare>();
                if(comp.squareOccupied)
                {
                    shapeCanBePlacedOnBoard = false;
                }

            }
            if(shapeCanBePlacedOnBoard)
            {
                canBePlaced = true;
            }
        }
        return canBePlaced;
            
    }

    private List<int[]> GetAllSquareesCombination(int cols, int rows)
    {
        var squareList = new List<int[]>();
        var lastColIndex = 0;
        var lastRowIndex = 0;

        int safeIndex = 0;

        while (lastRowIndex + (rows - 1) < 9)
        { 
            var rowdata=new List<int>();
            for (var row = lastRowIndex; row < lastRowIndex + rows; row++)
            {
                for (var col = lastColIndex; col < lastColIndex + cols; col++)
                {
                    rowdata.Add(_lineIndecator.line_Data[row,col]);
                }
            }
            squareList.Add(rowdata.ToArray());
            lastColIndex++;

            if(lastColIndex + (cols - 1) >= 9)
            {
                lastRowIndex++;
                lastColIndex = 0;
            }
            safeIndex++;
            if(safeIndex>100)
            {
                break;

            }
                
        }
        return squareList;
    }

    

}
