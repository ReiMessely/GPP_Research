using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGridManipulation : MonoBehaviour
{
    [SerializeField] private GameObject gameGridObject;
    private GameGrid gameGrid;
    [SerializeField] private GameObject agentPrefab;
    [SerializeField] private GameObject DEBUG_MousePosObject;
    [SerializeField] private LayerMask gridLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameGrid = gameGridObject.GetComponent<GameGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Debug.DrawRay(screenRay.origin, screenRay.direction * 1000, Color.green, 5);
            if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 1000, gridLayer))
            {
                GridCell cell = gameGrid.GetGridCell(gameGrid.GetGridPosFromWorld(hitInfo.point));
                if (cell)
                {
                    gameGrid.UpdateTarget(cell.GetPosition());
                }
                // DEBUG_SpawnDebugSphere(hitInfo.point, Color.green);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (Input.GetKey(KeyCode.E))
            {
                Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                // Debug.DrawRay(screenRay.origin, screenRay.direction * 1000, Color.yellow, 5);
                if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 1000, gridLayer))
                {
                    GridCell cell = gameGrid.GetGridCell(gameGrid.GetGridPosFromWorld(hitInfo.point));
                    if (cell)
                    {
                        cell.MakeImpassable();
                        gameGrid.UpdateTerrainCalcs();
                    }
                    // DEBUG_SpawnDebugSphere(hitInfo.point, Color.yellow);
                }
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 1000, gridLayer))
                {
                    GridCell cell = gameGrid.GetGridCell(gameGrid.GetGridPosFromWorld(hitInfo.point));
                    if (cell)
                    {
                        Instantiate(agentPrefab, hitInfo.point + Vector3.up, Quaternion.identity);
                    }
                }
            }
            else 
            {
                Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(screenRay, out RaycastHit hitInfo, 1000, gridLayer))
                {
                    GridCell cell = gameGrid.GetGridCell(gameGrid.GetGridPosFromWorld(hitInfo.point));
                    if (cell)
                    {
                        cell.MakePassable();
                        gameGrid.UpdateTerrainCalcs();
                    }
                }
            }
        }
    }

    private void DEBUG_SpawnDebugSphere(Vector3 worldPos, Color color)
    {
        if (DEBUG_MousePosObject)
        {
            GameObject obj = Instantiate(DEBUG_MousePosObject, worldPos, Quaternion.identity);
            obj.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
