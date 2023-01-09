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
    public bool impassable;

    [SerializeField] GameObject textObject;
    private TextMeshProUGUI tmpro;

    private void Awake()
    {
        // Initialise in Awake since grid is made in Start
        tmpro = textObject.GetComponent<TextMeshProUGUI>();

        direction.x = Random.value * 2 - 1;
        direction.y = 0;
        direction.z = Random.value * 2 - 1;
        direction.Normalize();
        cost = 1;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + direction * 5,Color.red);

        if (tmpro)
        {
            if (impassable)
            {
                tmpro.SetText("inf");
            }
            else
            {
                tmpro.SetText(cost.ToString());
            }
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
}
