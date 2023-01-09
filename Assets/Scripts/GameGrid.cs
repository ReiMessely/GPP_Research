using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameGrid : MonoBehaviour
{
    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] private float gridSpaceSize = 5f;

    [SerializeField] private GameObject gridCellPrefab;
    private GameObject[,] gameGridObjects;
    private GridCell[,] gridCells; 

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        foreach (GridCell cell in gridCells)
        {
            if ((int)(Random.value*100) % 11 == 0)
            {
                cell.impassable = true;
                cell.GetComponentInChildren<MeshRenderer>().material.color = Color.black;
            }
        }
        CreateIntegrationField(new Vector2Int(6,7));
        CreateFlowField();

        //PrintAllCellCosts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void PrintAllCellCosts()
    {
        foreach (GridCell cell in gridCells)
        {
            Debug.Log("[" + cell.GetPosition().x.ToString() + ", " + cell.GetPosition().y.ToString() + "] Cost: " + cell.cost);
        }
    }

    private void CreateGrid()
    {
        if (gridCellPrefab == null) 
        {
            Debug.Log("gridCellPrefab not assigned");
            return;
        }

        gameGridObjects = new GameObject[width, height];
        gridCells = new GridCell[width, height];
        for (int x = 0; x < width; x++) 
        {
            for (int y = 0; y < height; y++) 
            {
                gameGridObjects[x, y] = Instantiate(gridCellPrefab, new Vector3(x * gridSpaceSize, 0, y * gridSpaceSize), Quaternion.identity);
                gameGridObjects[x, y].transform.parent = transform;
                gameGridObjects[x,y].gameObject.name = "GameGridCell (X: " + x.ToString() + ", Y: " + y.ToString() + ")";
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1))
                {
                    gameGridObjects[x, y].GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
                }

                // Set up grid cell data
                gridCells[x, y] = null;
                gridCells[x, y] = gameGridObjects[x, y].GetComponent<GridCell>();
                gridCells[x, y].SetPosition(x, y);
                gridCells[x, y].cost = 1;
                gridCells[x, y].impassable = false;
            }
        }
    }

    public Vector2Int GetGridPosFromWorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.z / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float y = gridPos.y * gridSpaceSize;

        return new Vector3(x,0,y);
    }

    public bool IsInGrid(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height);
    }

    public GridCell GetGridCell(Vector2Int gridPos)
    {
        if (IsInGrid(gridPos))
            return gridCells[gridPos.x, gridPos.y];
        return null;
    }

    public GridCell GetGridCell(int x, int y)
    {
        return gridCells[x,y];
    }

    private void ResetFieldCost()
    {
        foreach (GridCell cell in gridCells)
        {
            cell.cost = int.MaxValue;
        }
    }

    private List<GridCell> GetNeighbours(Vector2Int gridPos)
    {
        List<GridCell> neighbours = new List<GridCell>();
        List<Vector2Int> gridPositions = new List<Vector2Int>
        {
            new Vector2Int(gridPos.x + 1, gridPos.y),
            new Vector2Int(gridPos.x + 1, gridPos.y + 1),
            new Vector2Int(gridPos.x + 1, gridPos.y - 1),
            new Vector2Int(gridPos.x, gridPos.y + 1),
            new Vector2Int(gridPos.x, gridPos.y - 1),
            new Vector2Int(gridPos.x - 1, gridPos.y),
            new Vector2Int(gridPos.x - 1, gridPos.y + 1),
            new Vector2Int(gridPos.x - 1, gridPos.y - 1)
        };

        foreach (Vector2Int pos in gridPositions)
        {
            if (IsInGrid(pos))
            {
                neighbours.Add(GetGridCell(pos));
            }
        }

        return neighbours;
    }

    private void CreateIntegrationField(Vector2Int targetCell)
    {
        ResetFieldCost();

        List<Vector2Int> openList = new List<Vector2Int>();
        bool[,] processed = new bool[width,height];
        gridCells[targetCell.x, targetCell.y].cost = 0;
        openList.Add(targetCell);
        GridCell targetGridCell = GetGridCell(targetCell);
        if (targetGridCell == null)
        {
            Debug.Log("Failed to retrieve targetGridCell");
            return;
        }
        while (openList.Count > 0) 
        {
            Vector2Int cell = openList.First();
            if (processed[cell.x, cell.y])
            {
                openList.RemoveAt(0);
                continue;
            }

            List<GridCell> neighbours = GetNeighbours(cell);

            foreach (GridCell neighbour in neighbours) 
            {
                float distance = (neighbour.gameObject.transform.position - targetGridCell.gameObject.transform.position).magnitude;
                if (distance < neighbour.cost && !neighbour.impassable)
                {
                    neighbour.cost = (int)distance;
                }
                if (!openList.Contains(neighbour.GetPosition()))
                {
                    openList.Add(neighbour.GetPosition());
                }
            }

            processed[cell.x, cell.y] = true;

            openList.RemoveAt(0);

            int smallestCost = int.MaxValue;
            foreach (GridCell gridCell in gridCells)
            {
                if (gridCell.cost < smallestCost)
                {
                    smallestCost = gridCell.cost;
                    break;
                }
            }
            if (smallestCost == int.MaxValue)
            {
                Debug.Log("aborted by processed or no path");
                break;
            }
        }
    }

    private void CreateFlowField()
    {
        foreach (GridCell cell in gridCells)
        {
            List<GridCell> neighbours = GetNeighbours(cell.GetPosition());
            GridCell lowestCostNeighbour = cell;
            foreach (GridCell neighbour in neighbours)
            {
                if (neighbour.cost < lowestCostNeighbour.cost) 
                {
                    lowestCostNeighbour = neighbour;
                }
            }
            if (lowestCostNeighbour == cell)
            {
                cell.direction = new Vector3(0, 0, 0);
                continue;
            }
            cell.direction = lowestCostNeighbour.gameObject.transform.position - cell.gameObject.transform.position;
            cell.direction.Normalize();
        }
    }
}
