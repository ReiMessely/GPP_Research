using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        
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
                gridCells[x, y] = gameGridObjects[x,y].GetComponent<GridCell>();
                gridCells[x, y].SetPosition(x, y);
                gameGridObjects[x, y].transform.parent = transform;
                gameGridObjects[x,y].gameObject.name = "GameGridCell (X: " + x.ToString() + ", Y: " + y.ToString() + ")";
                if ((x % 2 == 0 && y % 2 == 0) || (x % 2 == 1 && y % 2 == 1))
                {
                    gameGridObjects[x, y].GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
                }
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

    public GridCell GetGridCell(int x, int y)
    {
        return gridCells[x,y];
    }
}
