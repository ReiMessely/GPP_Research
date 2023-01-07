using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SimpleAgentMovement : MonoBehaviour
{
    public GameGrid gameGrid;
    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private LayerMask gridCellLayer;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if (gameGrid== null) 
        {
            gameGrid = FindObjectOfType<GameGrid>();
        }
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycast, 10, gridCellLayer))
        {
            Vector2Int xy = gameGrid.GetGridPosFromWorld(raycast.point);
            GridCell gc = gameGrid.GetGridCell(xy.x,xy.y);
            transform.position += gc.direction * movementSpeed * Time.deltaTime;
        }
    }
}
