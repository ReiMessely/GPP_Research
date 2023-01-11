using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using TMPro;

public class GridCell : MonoBehaviour
{
    private int posX;
    private int posY;

    public Vector3 direction;
    public int cost;
    public int totalCost;
    private bool impassable;

    [SerializeField] private TextMeshProUGUI tmpro;
    [SerializeField] private GameObject arrowImage;
    private MeshRenderer meshRenderer;

    private enum GridCellDisplayMode
    {
        TotalCost, Arrow, Cost
    }

    private GridCellDisplayMode displayMode = GridCellDisplayMode.TotalCost;

    private void Awake()
    {
        // Initialise in Awake since grid is made in Start
        tmpro.enabled = true;
        arrowImage.SetActive(false);
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        direction.x = Random.value * 2 - 1;
        direction.y = 0;
        direction.z = Random.value * 2 - 1;
        direction.Normalize();
        cost = 1;
        totalCost = 1;
        impassable = false;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + direction * 5,Color.red);

        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleDisplayMode();
        }
    }

    public void SetPosition(int x, int y)
    {
        posX = x;
        posY = y;
    }

    public Vector2Int GetPosition()
    {
        return new Vector2Int(posX, posY);
    }

    public void MakeImpassable()
    {
        impassable = true;
        cost = int.MaxValue;
        totalCost = int.MaxValue;
        ChangeColor(Color.black);
    }

    public void MakePassable()
    {
        impassable = false;
        cost = 1;
        totalCost = int.MaxValue;
    }

    public bool IsImpassable()
    {
        return impassable;
    }

    public void ChangeColor(Color color)
    {
        meshRenderer.material.color = color;
    }

    private void CycleDisplayMode()
    {
        // Cycle through
        switch (displayMode)
        {
            case GridCellDisplayMode.Cost:
                displayMode = GridCellDisplayMode.TotalCost;
                break;
            case GridCellDisplayMode.TotalCost:
                displayMode = GridCellDisplayMode.Arrow;
                break;
            case GridCellDisplayMode.Arrow:
                displayMode = GridCellDisplayMode.Cost;
                break;
        }

        // Do stuff depending on display mode
        switch (displayMode)
        {
            case GridCellDisplayMode.Cost:
                tmpro.enabled = true;
                arrowImage.SetActive(false);
                UpdateDisplay();
                break;
            case GridCellDisplayMode.TotalCost:
                tmpro.enabled = true;
                arrowImage.SetActive(false);

                UpdateDisplay();
                break;
            case GridCellDisplayMode.Arrow:
                tmpro.enabled = false;

                UpdateDisplay();
                break;
        }
    }

    public void UpdateDisplay()
    {
        switch (displayMode)
        {
            case GridCellDisplayMode.Cost:
                if (impassable)
                {
                    tmpro.SetText("inf");
                }
                else
                {
                    tmpro.SetText(cost.ToString());
                }
                break;
            case GridCellDisplayMode.TotalCost:
                if (impassable)
                {
                    tmpro.SetText("inf");
                }
                else if (totalCost == int.MaxValue)
                {
                    tmpro.SetText("unav");
                }
                else
                {
                    tmpro.SetText(totalCost.ToString());
                }
                break;
            case GridCellDisplayMode.Arrow:
                if (tmpro.enabled)
                {
                    break;
                }

                if (direction.magnitude <= float.Epsilon)
                {
                    arrowImage.SetActive(false);
                }
                else
                {
                    arrowImage.SetActive(true);
                }

                Vector3 newEuler = new Vector3(arrowImage.transform.rotation.eulerAngles.x, arrowImage.transform.rotation.eulerAngles.y, arrowImage.transform.rotation.eulerAngles.z);
                newEuler.z = Mathf.Rad2Deg * Mathf.Atan2(direction.z, direction.x) - 45;
                arrowImage.transform.eulerAngles = newEuler;
                break;
        }
    }
}
