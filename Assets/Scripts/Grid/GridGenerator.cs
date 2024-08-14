using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : InGameSingleton<GridGenerator>
{
    #region Private Field
    private Grid grid;
    private Vector2Int[] eightDirections = new Vector2Int[]
    {
        new Vector2Int(0, 1),    // 위
        new Vector2Int(1, 1),    // 오른쪽 위 대각선
        new Vector2Int(1, 0),    // 오른쪽
        new Vector2Int(1, -1),   // 오른쪽 아래 대각선
        new Vector2Int(0, -1),   // 아래
        new Vector2Int(-1, -1),  // 왼쪽 아래 대각선
        new Vector2Int(-1, 0),   // 왼쪽
        new Vector2Int(-1, 1)    // 왼쪽 위 대각선
    };
    #endregion
    #region Serilize Field
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;
    [SerializeField] private float cellSize;
    [SerializeField] private bool printCells;
    [SerializeField] private Vector2 gridOrigin;
    #endregion
    #region Public Properties
    public Grid Grid { get { return grid; } }
    #endregion
    #region MonoBehaviour Callbacks
    private void Start()
    {
        grid = new Grid(sizeX, sizeY, cellSize, gridOrigin);
        if (printCells)
        {
            StartCoroutine("PrintGrid");
        }
    }
    #endregion
    #region Public Methods
    public Vector2Int FindNearestValidGridPosition(Vector2Int position)
    {
        if (grid.IsGridAvailable(position))
        {
            return position;
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
        queue.Enqueue(position);
        visited.Add(position);

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            foreach (var dir in eightDirections)
            {
                Vector2Int neighbor = current + dir;
                if (grid.IsGridAvailable(neighbor))
                {
                    return neighbor;
                }
                if (grid.IsValidGridPosition(neighbor) && !visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        // 유효한 그리드 위치를 찾지 못한 경우 원래 위치 반환
        return position;
    }
    #endregion
    #region Private Methods
    private IEnumerator PrintGrid()
    {
        while (true)
        {
            grid.PrintGrid();
            yield return new WaitForSeconds(0.2f);
        }
    }
    #endregion
}
