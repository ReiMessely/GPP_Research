using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private int posX;
    private int posY;

    public Vector3 direction;

    private void Start()
    {
        direction.x = Random.value * 2 - 1;
        direction.y = 0;
        direction.z = Random.value * 2 - 1;
        direction.Normalize();
    }

    private void Update()
    {
        Debug.DrawLine(transform.position + Vector3.up * 0.1f, transform.position + direction * 5,Color.red);
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
