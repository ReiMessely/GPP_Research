using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimpleAgentMovement : MonoBehaviour
{
    public GameGrid gameGrid;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private LayerMask gridCellLayer;
    private Vector3 currentDirection;
    private float directionChangeRange = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentDirection = new Vector3(0, 0, 0);
        if (gameGrid== null) 
        {
            gameGrid = FindObjectOfType<GameGrid>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycast, 10, gridCellLayer))
        {
            Vector2Int xy = gameGrid.GetGridPosFromWorld(raycast.point);
            Vector3 centerOfCell = gameGrid.GetWorldPosFromGridPos(xy);
            if ((centerOfCell - transform.position).sqrMagnitude <= (directionChangeRange * directionChangeRange))
            {
                GridCell gc = gameGrid.GetGridCell(xy.x,xy.y);
                if (gc)
                {
                    currentDirection = gc.direction;
                }
            }
            transform.position += currentDirection * movementSpeed * Time.deltaTime;
        }
    }
}
