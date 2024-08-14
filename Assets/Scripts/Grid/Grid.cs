using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Grid
{
    #region Private Fields
    private int width;
    private int height;
    private float cellSize;
    private bool[,] gridArray; // true는 이동 가능, false는 불가능 구역
    private Vector2 origin;
    #endregion

    #region Constructors
    public Grid(int width, int height, float cellSize, Vector2? origin = null)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin ?? Vector2.zero;

        gridArray = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = true;
            }
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 월드 좌표 -> 그리드 좌표
    /// 월드 좌표를 받아서 가장 가까운 그리드 좌표를 그리드에서 활용할 수 있는 좌표로 돌려줌
    /// </summary>
    public Vector2Int World2Grid(Vector2 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition - origin).x / cellSize);
        int y = Mathf.RoundToInt((worldPosition - origin).y / cellSize);

        x = Mathf.Clamp(x, 0, width - 1);
        y = Mathf.Clamp(y, 0, height - 1);

        return new Vector2Int(x, y);
    }
    /// <summary>
    /// 그리드 좌표 -> 월드 좌표
    /// 그리드 좌표를 받아서 해당 좌표를 월드에서 쓸 수 있는 좌표로 돌려줌
    /// </summary>
    public Vector2 Grid2World(int x, int y)
    {
        return new Vector2(x, y) * cellSize + origin;
    }
    /// <summary>
    /// 그리드 좌표 -> 월드 좌표
    /// 그리드 좌표를 받아서 해당 좌표를 월드에서 쓸 수 있는 좌표로 돌려줌
    /// </summary>
    public Vector2 Grid2World(Vector2Int gridCoordinate)
    {
        return Grid2World(gridCoordinate.x, gridCoordinate.y);
    }
    /// <summary>
    /// 월드 좌표 -> 월드 그리드 좌표
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    public Vector2 World2WorldGrid(Vector2 worldPosition)
    {
        return Grid2World(World2Grid(worldPosition));
    }
    public Vector2 WorldPositionInBound(Vector2 worldPosition)
    {
        Vector2 worldLimit = Grid2World(width - 1, height - 1);
        worldPosition.x = Mathf.Clamp(worldPosition.x, -worldLimit.x, worldLimit.x);
        worldPosition.y = Mathf.Clamp(worldPosition.y, -worldLimit.y, worldLimit.y);
        return worldPosition;
    }

    /// <summary>
    /// 해당 그리드가 유효하고 이동 가능한지 확인
    /// 유효 : 범위 검사
    /// 이동 가능 : 그리드 값이 true인가 false인가
    /// </summary>
    public bool IsGridAvailable(Vector2Int position)
    {
        return IsValidGridPosition(position) && gridArray[position.x, position.y];
    }
    /// <summary>
    /// 그리드에 이동 가/불가를 설정하는 함수
    /// true로 하면 이동 가능지역으로, false로 하면 불가능 지역으로 설정
    /// </summary>
    /// <param name="gridPosition">월드 상의 위치</param>
    /// <param name="value">이동 가능 여부</param>
    public void SetGridValue(Vector2 position, bool value)
    {
        Vector2Int gridPosition = World2Grid(position);
        gridArray[gridPosition.x, gridPosition.y] = value;
    }
    /// <summary>
    /// 그리드를 화면상에 출력하는 함수
    /// </summary>
    public void PrintGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y + 1 < height)
                {
                    PrintLine(x, y, x, y + 1);
                }
                if (x + 1 < width)
                {
                    PrintLine(x, y, x + 1, y);
                }
            }
        }

        // 상단 테두리
        for (int x = 0; x < width - 1; x++)
        {
            PrintLine(x, height - 1, x + 1, height - 1);
        }

        // 우측 테두리
        for (int y = 0; y < height - 1; y++)
        {
            PrintLine(width - 1, y, width - 1, y + 1);
        }
    }
    /// <summary>
    /// 그리드 포지션이 유효한지 확인 (범위만 벗어나지 않는지 확인)
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
    }
    #endregion
    #region Private Methods
    // 이동 가능한지 판단하여 줄 색깔 출력
    private void PrintLine(int startX, int startY, int endX, int endY)
    {
        Vector2 startPos = Grid2World(startX, startY);
        Vector2 endPos = Grid2World(endX, endY);
        Color color = Color.white;

        if (!IsGridAvailable(new Vector2Int(startX, startY)) || !IsGridAvailable(new Vector2Int(endX, endY)))
        {
            color = Color.red;
        }

        Debug.DrawLine(startPos, endPos, color, 0.2f);
    }

    // 그리드 테두리를 이동 불가 지역으로 설정
    private void BlockOutline()
    {
        for (int x = 0; x < width; x++)
        {
            gridArray[x, 0] = false; // 하단 테두리
            gridArray[x, height - 1] = false; // 상단 테두리
        }

        for (int y = 0; y < height; y++)
        {
            gridArray[0, y] = false; // 좌측 테두리
            gridArray[width - 1, y] = false; // 우측 테두리
        }
    }
    #endregion
}
