using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform cellsParent;

    [SerializeField] private GameObject cellPrefab;

    public int gridSize;

    private GameObject[,] grid;

    private float cellSize = 1;

    public static GridManager Instance;

    private void Awake()
    {
        Instance = this;

        UIManager.Instance.OnRebuildButtonClicked += RebuildGrid;
    }

    private void Start()
    {
        BuildGrid();
    }

    #region Grid System

    public void RebuildGrid(int _size)
    {
        DeleteGrid();

        gridSize = _size;

        BuildGrid();
    }

    private void BuildGrid()
    {
        grid = new GameObject[gridSize, gridSize];

        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                grid[j, i] = Instantiate(cellPrefab, new Vector3(j * cellSize, i * cellSize, 0), Quaternion.identity);

                grid[j, i].GetComponent<Cell>().SetPosition(j, i);

                grid[j, i].GetComponent<Cell>().isSelected = false;

                grid[j, i].transform.parent = cellsParent;
            }
        }
    }

    private void DeleteGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                Destroy(grid[j, i]);
            }
        }
    }

    #endregion

    #region Grid Cell Functions

    public bool CheckOnCell(Vector2 cellPos)
    {
        if (cellPos.x >= 0 && cellPos.x < gridSize && cellPos.y >= 0 && cellPos.y < gridSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ActivateCell(Vector2 cellPos)
    {
        GetCell(cellPos).GetComponent<Cell>().SelectCell();

        CheckAndDeactivateCells(cellPos);
    }

    private void CheckAndDeactivateCells(Vector2 lastCellPos)
    {
        List<Vector2> connectedCells = new List<Vector2>();

        connectedCells.Add(lastCellPos);

        while (true)
        {
            int initCount = connectedCells.Count;

            List<Vector2> newCells = new List<Vector2>();

            foreach (var connected in connectedCells)
            {
                foreach (var neighbor in GetNeighbors(connected))
                {
                    if (!connectedCells.Contains(neighbor) && !newCells.Contains(neighbor))
                    {
                        newCells.Add(neighbor);
                    }
                }
            }

            connectedCells.AddRange(newCells);

            if (initCount == connectedCells.Count)
                break;
        }

        if (connectedCells.Count > 2)
        {
            PlayerPrefs.SetInt("MatchCount", PlayerPrefs.GetInt("MatchCount") + 1);

            UIManager.Instance.OnMatchCountChanged?.Invoke();

            foreach (var item in connectedCells)
            {
                StartCoroutine(GetCell(item).ResetCell(0.2f));
            }
        }
    }

    private List<Vector2> GetNeighbors(Vector2 gridPos)
    {
        List<Vector2> AllNeighbors = new List<Vector2>(); // Vertical and Horizontal neighbors list

        if (gridPos.y + 1 < gridSize)
        {
            Cell cell = GetCell(gridPos + new Vector2(0, 1));

            if (cell.isSelected)
            {
                AllNeighbors.Add(cell.GetPosition());
            }
        }

        if (gridPos.y - 1 >= 0)
        {
            Cell cell = GetCell(gridPos + new Vector2(0, -1));

            if (cell.isSelected)
            {
                AllNeighbors.Add(cell.GetPosition());
            }
        }

        if (gridPos.x + 1 < gridSize)
        {
            Cell cell = GetCell(gridPos + new Vector2(1, 0));

            if (cell.isSelected)
            {
                AllNeighbors.Add(cell.GetPosition());
            }
        }

        if (gridPos.x - 1 >= 0)
        {
            Cell cell = GetCell(gridPos + new Vector2(-1, 0));

            if (cell.isSelected)
            {
                AllNeighbors.Add(cell.GetPosition());
            }
        }

        return AllNeighbors;
    }
    public Vector2 GetGridPos(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);

        return new Vector2(x, y);
    }

    public Cell GetCell(Vector2 gridPos)
    {
        int x = Mathf.RoundToInt(gridPos.x);
        int y = Mathf.RoundToInt(gridPos.y);

        return grid[x, y].GetComponent<Cell>();
    }

    #endregion
}
